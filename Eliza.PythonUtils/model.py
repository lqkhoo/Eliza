# This module contains models that serialize the bytestream
# of a decrypted game save into structs and primitives.
# Note that at this level, we don't have semantic representation
# for complex objects.

# This model duplicates the structs from the hex templates in a Python script
# so can can do downstream processing.

# We can easily construct separate models for each version of the game,
# to handle changes across versions by just inheriting from existing models

from construct import (this, Construct, Probe, Struct,
                        RepeatUntil, If, IfThenElse,
                        PaddedString,
                        Flag,    # Single-byte boolean
                        Int8ul,  # byte / char / uint8
                        Int16ul, # ushort / uint16
                        Int32sl, # int / int32
                        Int32ul, # uint32
                        Int64sl, # int64
                        Int64ul, # uint64
                        Float32l # float
                    )

# If we know the length of the data of a specific input (due to variable
# lengths, this cannot be for all input), we can stub out a struct for unit tests
# by declaring a field with Int8ul[length], which is just a dummy array of bytes

# To debug a Struct, use Probe()
# https://construct.readthedocs.io/en/latest/debugging.html#probe

# Common ----------------

class IModel(object):
    pass # Nothing. Just used for type hints


class BaseModel(IModel):
    def __init__(self):
        super().__init__()

        self.Struct_Vector3: Struct = Struct(
            "x" / Float32l,
            "y" / Float32l,
            "z" / Float32l
        )

        self.Struct_Vector3Int: Struct = Struct(
            "x" / Int32sl,
            "y" / Int32sl,
            "z" / Int32sl
        )

        self.Struct_Quaternion: Struct = Struct(
            "x" / Float32l,
            "y" / Float32l,
            "z" / Float32l,
            "w" / Float32l
        )

        self.Struct_SAVEFLAG: Struct = Struct(
            "length" / Int32sl, # number of *bits*
            "data" / Int8ul[((this.length +7 ) & (-8)) >> 3] # Align length to bytes, then divide by 8 to get nbytes
        )

        self.Struct_MESSAGEPACKOBJ: Struct = Struct(
            "length" / Int32sl, # number of *bytes*
            # "data" / PaddedString(this.length, 'utf-8') # Doesn't work
            "data" / Int8ul[this.length]
        )

        # Basically array of self.Struct_MESSAGEPACKOBJ
        self.Struct_MESSAGEPACKOBJLIST: Struct = Struct(
            "count" / Int32sl,
            "container" / Struct(
                "obj" / self.Struct_MESSAGEPACKOBJ[this._.count]
            )
        )



        # RF5_Header -----------------

        self.Struct_SAVETIME: Struct = Struct(
            "year" / Int16ul,
            "day" / Int16ul,
            "month" / Int8ul,
            "hour" / Int8ul,
            "minute" / Int8ul,
            "second" / Int8ul
        )

        self.Struct_RF5_HEADER: Struct = Struct(
            "uid" / Int64ul,
            "version" / Int32ul,
            "project" / PaddedString(4, 'utf-8'),
            "WCnt" / Int32ul,
            "WOpt" / Int32ul,
            "SAVETIME" / self.Struct_SAVETIME
        )



        # RF5_Footer ------------------

        self.Struct_RF5_FOOTER: Struct = Struct(
            "bodyLength" / Int32sl,
            "length" / Int32sl,
            "sum" / Int32ul,
            "blank" / Int32sl # Not sure what this is. Not always zero
        )



        # RF5_Data -------------------

        # RF5_WORLDDATA --------------------

        self.Struct_WEATHERDATA: Struct = Struct(
            "weatherId" / Int8ul,
            "weatherDayId" / Int8ul,
            "todayRate" / Int8ul,
            "typhoonDayCount" / Int8ul,
            "runeyDayCount" / Int8ul,
            "meteorShowerDayCount" / Int8ul,
            "nextWeatherDayId" / Int8ul,
            "weatherHourCount" / Int8ul,
            "weatherHour" / Int8ul[12]
        )

        self.Struct_MININGPOINTSAVEDATA: Struct = Struct(
            "farmId" / Int32sl,
            "uid" / Int32sl,
            "position" / self.Struct_Vector3,
            "cropId" / Int32sl,
            "mineTypeId" / Int32sl,
            "itemId" / Int32sl,
            "hp" / Int32sl
        )

        self.Struct_REWARDITEMDATA: Struct = Struct(
            "itemId" / Int32sl,
            "amount" / Int32sl,
            "level" / Int32sl
        )

        self.Struct_ORDERREWARDRECIPEDATA: Struct = Struct(
            "data" / Int32sl # Unknown
        )

        self.Struct_REWARDBOXSAVEDATA: Struct = Struct(
            "rewardItemDataCount" / Int32sl,
            "rewardItemData" / self.Struct_REWARDITEMDATA[this.rewardItemDataCount],
            "orderRewardRecipeDataCount" / Int32sl,
            "orderRewardRecipeData" / self.Struct_ORDERREWARDRECIPEDATA[this.orderRewardRecipeDataCount],
            "orderRewardGold" / Int32sl,
            "wantedRewardItemDataCount" / Int32sl,
            "wantedRewardItemData" / self.Struct_REWARDITEMDATA[this.wantedRewardItemDataCount],
            "festivalRewardItemDataCount" / Int32sl,
            "festivalRewardItemData" / self.Struct_REWARDITEMDATA[this.festivalRewardItemDataCount]
        )

        self.Struct_RF5_WORLDDATA: Struct = Struct(
            "difficultyValue" / Int8ul,
            "scenarioStoppedTime" / Int32sl,
            "mapId" / Int32sl,
            "position" / self.Struct_Vector3,
            "rotationEuler" / self.Struct_Vector3,
            "inGameTimeElapsedTime" / Int32sl,
            "weatherData" / self.Struct_WEATHERDATA,
            "shopRandSeedFix" / Int32ul,
            "shopRandSeed" / Int32ul,
            "shopSeedGenerateDay" / Int32sl, # Can be -1
            "lastShippingDay" / Int32sl,
            "lastPlaceId" / Int32sl,
            "lastSleepTime" / Int32sl,
            "miningPointSaveDataCount" / Int32sl,
            "miningPointSaveData" / self.Struct_MININGPOINTSAVEDATA[this.miningPointSaveDataCount],
            "rewardBoxSaveData" / self.Struct_REWARDBOXSAVEDATA,
            "itemSpawnFlag" / self.Struct_SAVEFLAG,
            "treasureFlag" / self.Struct_SAVEFLAG,
            "gimmickFlag" / self.Struct_SAVEFLAG,
            "seedPointElapsedDay" / Int32ul,
            "seedPointMonsterAddedCount" / Int32ul,
            "seedSupportCoolTime" / Float32l,
            "meteorPositionCount" / Int32sl,
            "meteorPosition" / Int32sl[lambda x: max(this.meteorPositionCount, 1)], # need the lambda, otherwise 1 is a Struct for some reason
            "offsetFiveYearsAgo" / Int32sl,
            "punchCount" / Int32sl
        )



        # RF5_PLAYERDATA --------------------

        self.Struct_SKILLDATA: Struct = Struct(
            "exp" / Int32sl,
            "level" / Int32sl
        )

        self.Struct_RF5_PLAYERDATA: Struct = Struct(
            "playerGold" / Int32sl,
            "playerNameCount" / Int32sl,
            # "playerName" / PaddedString(0x20, 'utf-8'), # Doesn't work. Invalid byte
            "playerName" / Int8ul[0x20],
            "isPlayerMale" / Flag,
            "isPlayerModelMale" / Flag,
            "variationNo" / Int32sl,
            "playerBirthday" / Int32sl, # In base 10. hundreth integer is month.
                                        # Rest is day. e.g. 215 = 15th of autumn. 008 = 8th of spring.
            "marriedNpcId" / Int32ul, # 0 is unmarried
            "seedPoint" / Int32sl,
            "policeRank" / Int32sl,
            "stone" / Int32sl,
            "lumber" / Int32sl,
            "compost" / Int32sl,
            "esa" / Int32sl, # animal feed
            "dailyRecipePanBakery" / Int32sl, # No. that has been bought already, not remaining
            "dailyRecipePanRestaurant" / Int32sl,
            "bathroomBlocked" / Int32sl,
            "skillDataCount" / Int32sl,
            "skillData" / self.Struct_SKILLDATA[this.skillDataCount],
            "dualSmithActor" / Int32sl, # Can be -1
            "dualCookActor" / Int32sl, # Can be -1
            "dualFishingActor" / Int32sl # Can be -1
        )



        # RF5_EVENTDATA ---------------------

        self.Struct_EVENTSAVEPARAMETER: Struct = Struct(
            "objects" / self.Struct_MESSAGEPACKOBJ
        )

        self.Struct_SUBEVENTSAVEDATA: Struct = Struct(
            "objects" / self.Struct_MESSAGEPACKOBJ
        )

        self.Struct_RF5_EVENTDATA: Struct = Struct(
            "eventSaveParameter" / self.Struct_EVENTSAVEPARAMETER,
            "saveFlag" / self.Struct_SAVEFLAG,
            "subEventSaveDatas" / self.Struct_SUBEVENTSAVEDATA,
            "mainScenarioStep" / Int32sl,
            "presentSendActorListCount" / Int32sl,
            "presentRecvActorListCount" / Int32sl,
            "isStartFishing" / Int8ul,
            "fesJoinActorListCount" / Int32sl,
            "fesVisitorActorListCount" / Int32sl,
            "fesNpcScoreListCount" / Int32sl,
            "isCalcFesId" / Int32sl,
            "victoryCandidateNpcId" / Int32sl, # Can be -1
            "judgeChildId" / Int32sl,
            "fishTypeListCount" / Int32sl
        )

        # RF5_NPCDATA ------------------

        self.Struct_CHILDSAVEDATA: Struct = Struct(
            "count" / Int32sl,
            "childSaveDatas" / Struct(
                "childData" / self.Struct_MESSAGEPACKOBJ[this._.count]
            )
        )

        self.Struct_NPCHATCACHE: Struct = Struct(
            "npcId" / Int32sl,
            "obj" / self.Struct_MESSAGEPACKOBJ
        )

        self.Struct_RF5_NPCDATA: Struct = Struct(
            "count" / Int32sl,
            "npcSaveParameters" / Struct(
                "npcSaveParameters" / self.Struct_MESSAGEPACKOBJ[this._.count]
            ),
            "npcDateSaveParam" / self.Struct_MESSAGEPACKOBJ,
            "childSaveDatas" / self.Struct_CHILDSAVEDATA,
            "giveBirthSaveParameter" / self.Struct_MESSAGEPACKOBJ,
            "npcHatCacheCount" / Int32sl,
            "npcHatCache" / self.Struct_NPCHATCACHE[this.npcHatCacheCount]
        )



        # RF5_FISHINGDATA ------------------

        self.Struct_FISHRECORD: Struct = Struct(
            "id" / Int32sl,
            "size" / self.Struct_Vector3Int
        )

        self.Struct_RF5_FISHINGDATA: Struct = Struct(
            "fishRecordCount" / Int32sl,
            "fishRecord" / self.Struct_FISHRECORD[this.fishRecordCount]
        )



        # RF5_STAMPDATA ----------------------

        self.Struct_STAMPRECORDDATA: Struct = Struct(
            "stampLevel" / Int32sl,
            "maxRecord" / Float32l,
            "minRecord" / Float32l
        )


        self.Struct_RF5_STAMPDATA: Struct = Struct(
            "stampRecordDataCount" / Int32sl,
            "stampRecordData" / self.Struct_STAMPRECORDDATA[80] # This is a fixed 80
        )



        # RF5_FURNITUREDATA ---------------------

        self.Struct_UNIQUEID: Struct = Struct(
            "char" / Int8ul[2]
        )

        self.Struct_STRINGID: Struct = Struct(
            # The way this works is that we keep reading until we find a sentinel value of two zero bytes.
            "uniqueId" / RepeatUntil(lambda x,lst,ctx: (lst[-1].char[0] == 0 and
                                                        lst[-1].char[1] == 0), self.Struct_UNIQUEID)
        )

        self.Struct_FURNITURESAVEDATA: Struct = Struct(
            "pos" / self.Struct_Vector3,
            "rot" / self.Struct_Quaternion,
            "sceneId" / Int32sl,
            "id" / Int32sl,
            "stringId" / self.Struct_STRINGID,
            "point" / Int32sl,
            "hp" / Int32sl,
            "have" / Int8ul
        )

        self.Struct_RF5_FURNITUREDATA: Struct = Struct(
            "unknown0" / Int32sl,
            "unknown1" / Int32sl,
            "furnitureSaveDataCount" / Int32sl,
            "furnitureSaveData" / self.Struct_FURNITURESAVEDATA[this.furnitureSaveDataCount]
        )



        # RF5_PARTYDATA ------------

        self.Struct_RF5_PARTYDATA: Struct = Struct(
            "partyDataCount" / Int32sl,
            "partyData" / Struct(
                "obj" / self.Struct_MESSAGEPACKOBJ[this._.partyDataCount]
            )
        )


        # RF5_ITEMDATA ------------

        self.Struct_RF5_ITEMDATA: Struct = Struct(
            "rucksack" / self.Struct_MESSAGEPACKOBJ,
            "itemBox" / self.Struct_MESSAGEPACKOBJ,
            "refrigerator" / self.Struct_MESSAGEPACKOBJ,
            "runeRuck" / self.Struct_MESSAGEPACKOBJ,
            "weaponBox" / self.Struct_MESSAGEPACKOBJ,
            "armorBox" / self.Struct_MESSAGEPACKOBJ,
            "farmToolBox" / self.Struct_MESSAGEPACKOBJ,
            "runeBox" / self.Struct_MESSAGEPACKOBJ,
            "shippingBox" / self.Struct_MESSAGEPACKOBJ,
            "fieldOnGroundStorage" / self.Struct_MESSAGEPACKOBJ
        )


        # RF5_FARMSUPPORTMONSTERDATA -------------

        self.Struct_FARMSUPPORTMONSTERDATALIST: Struct = Struct(
            "friendMonsterDataId" / Int32ul,
            "state" / Int32sl,
            "workTime" / Int32sl,
            "cellIndex" / Int32sl,
            "position" / self.Struct_Vector3,
            "rotation" / self.Struct_Quaternion
        )

        self.Struct_RF5_FARMSUPPORTMONSTERDATA: Struct = Struct(
            "farmSupportMonsterDataListCount" / Int32sl,
            "farmSupportMonsterDataLists" / self.Struct_FARMSUPPORTMONSTERDATALIST[
                                                    this.farmSupportMonsterDataListCount],
        )


        # RF5_FARMDATA ------------------

        self.Struct_RF5_FARMDATA: Struct = Struct(
            "firstVisitFarm" / self.Struct_MESSAGEPACKOBJLIST,
            "farmSizeLevels" / self.Struct_MESSAGEPACKOBJLIST,
            "farmCropDatas" / self.Struct_MESSAGEPACKOBJLIST,
            "crystalUseCounts" / self.Struct_MESSAGEPACKOBJLIST,
            "crop" / self.Struct_MESSAGEPACKOBJLIST,
            "soil" / self.Struct_MESSAGEPACKOBJLIST,
            "harvestCount" / self.Struct_MESSAGEPACKOBJLIST,
            "itemHarvestIdList" / self.Struct_MESSAGEPACKOBJLIST,
            "monsterHutReleaseFlags" / self.Struct_MESSAGEPACKOBJLIST,
            "monsterHutSaveDatas" / self.Struct_MESSAGEPACKOBJLIST
        )

        # RF5_STATUSDATA ----------------

        self.Struct_HUMANSTATUSDATA: Struct = Struct(
            "npcId" / Int32sl,
            "data" / self.Struct_MESSAGEPACKOBJ
        )

        # This was for when 1.0.2 and 1.0.6 saves were mistakenly kludged together
        """
        self.Struct_RF5_STATUSDATA: Struct = Struct(
            "count" / Int32sl,
            "dd" / If(this.count != 0x20, Int8ul[this.count]),
            "count2" / If(this.count != 0x20, Int32sl),
            "humanStatusData" / IfThenElse(this.count != 0x20,
                    self.Struct_HUMANSTATUSDATA[this.count2],
                    self.Struct_HUMANSTATUSDATA[this.count]
                    ),
            "friendMonsterStatus" / self.Struct_MESSAGEPACKOBJLIST
        )
        """

        self.Struct_RF5_STATUSDATA: Struct = Struct(
            "unknown" / self.Struct_MESSAGEPACKOBJ,
            "count" / Int32sl,
            "humanStatusData" / self.Struct_HUMANSTATUSDATA[this.count],
            "friendMonsterStatusDatas" / self.Struct_MESSAGEPACKOBJLIST
        )


        # RF5_ORDERDATA --------------------

        self.Struct_RF5_ORDERDATA: Struct = Struct(
            "orderSaveParameters" / self.Struct_MESSAGEPACKOBJ,
            "wantedSaveData" / self.Struct_MESSAGEPACKOBJ
        )

        #RF5_MAKINGDATA --------------------

        self.Struct_RF5_MAKINGDATA: Struct = Struct(
            "saveFlag" / self.Struct_SAVEFLAG,
            "enemyLevelupStage" / Int32sl,
            "makingCount" / Int32sl,
            "doEndScriptFlags" / self.Struct_SAVEFLAG
        )

        #RF5_POLICEBATCHDATA -------------

        self.Struct_RF5_POLICEBATCHDATA: Struct = Struct(
            "policeBatchIdSlotA" / Int32sl,
            "policeBatchIdSlotB" / Int32sl,
            "saveFlag" / self.Struct_SAVEFLAG
        )

        #RF5_ITEMFLAGDATA --------------

        self.Struct_RF5_ITEMFLAGDATA: Struct = Struct(
            "saveFlag" / self.Struct_SAVEFLAG,
            "clothFlag" / self.Struct_SAVEFLAG
        )

        #RF5_BUILDDATA -----------------

        self.Struct_RF5_BUILDDATA: Struct = Struct(
            "saveFlag" / self.Struct_SAVEFLAG
        )

        #RF5_SHIPPINGDATA --------------

        self.Struct_RF5_SHIPPINGDATA: Struct = Struct(
            "completedPercent" / Int32sl,
            "totalIncome" / Int64sl,
            "itemRecordListCount" / self.Struct_MESSAGEPACKOBJLIST[8],
            "fishShipmentRecords" / self.Struct_MESSAGEPACKOBJLIST,
            "seedLevelRecordList" / self.Struct_MESSAGEPACKOBJLIST,
            "syncToCampEquip" / Int8ul,
            "quickLastFocus" / Int32sl,
            "campLastFocus" / Int32sl,
            "unknown" / Int32sl[38],
            "farm1" / Int8ul[36], # char. names?
            "farm2" / Int8ul[36],
            "farm3" / Int8ul[36],
            "farm4" / Int8ul[36],
            "farm5" / Int8ul[36],
            "farm6" / Int8ul[36],
            "farm7" / Int8ul[36],
            "farm8" / Int8ul[36],
            "farm9" / Int8ul[36],
            "farm10" / Int8ul[36],
            "farm11" / Int8ul[36],
            "farm12" / Int8ul[36],
            "farm13" / Int8ul[36],
            "farm14" / Int8ul[36],
            "farm15" / Int8ul[36]
        )

        self.Struct_RF5_DATA: Struct = Struct(
            "slotNo" / Int32sl,
            "saveFlag" / self.Struct_SAVEFLAG,
            "worldData" / self.Struct_RF5_WORLDDATA,
            "playerData" / self.Struct_RF5_PLAYERDATA,
            "eventData" / self.Struct_RF5_EVENTDATA,
            "npcData" / self.Struct_RF5_NPCDATA,
            "fishingData" / self.Struct_RF5_FISHINGDATA,
            "stampData" / self.Struct_RF5_STAMPDATA,
            "furnitureData" / self.Struct_RF5_FURNITUREDATA,
            "partyData" / self.Struct_RF5_PARTYDATA,
            "itemData" / self.Struct_RF5_ITEMDATA,
            "farmSupportMonsterData" / self.Struct_RF5_FARMSUPPORTMONSTERDATA,
            "farmData" / self.Struct_RF5_FARMDATA,
            "statusData" / self.Struct_RF5_STATUSDATA,
            "orderData" / self.Struct_RF5_ORDERDATA,
            "makingData" / self.Struct_RF5_MAKINGDATA,
            "policeBatchData" / self.Struct_RF5_POLICEBATCHDATA,
            "itemFlagData" / self.Struct_RF5_ITEMFLAGDATA,
            "buildData" / self.Struct_RF5_BUILDDATA,
            "shippingData" / self.Struct_RF5_SHIPPINGDATA
        )


class Model_JP_1_0_6(BaseModel):
    def __init__(self):
        super().__init__()


class Model_JP_1_0_2(BaseModel):
    def __init__(self):
        super().__init__()

        self.Struct_RF5_STATUSDATA: Struct = Struct(
            # Removed Struct_MESSAGEPACKOBJ
            "count" / Int32sl,
            "humanStatusData" / self.Struct_HUMANSTATUSDATA[this.count],
            "friendMonsterStatusDatas" / self.Struct_MESSAGEPACKOBJLIST
        )

        self.Struct_RF5_DATA: Struct = Struct( # No change
            "slotNo" / Int32sl,
            "saveFlag" / self.Struct_SAVEFLAG,
            "worldData" / self.Struct_RF5_WORLDDATA,
            "playerData" / self.Struct_RF5_PLAYERDATA,
            "eventData" / self.Struct_RF5_EVENTDATA,
            "npcData" / self.Struct_RF5_NPCDATA,
            "fishingData" / self.Struct_RF5_FISHINGDATA,
            "stampData" / self.Struct_RF5_STAMPDATA,
            "furnitureData" / self.Struct_RF5_FURNITUREDATA,
            "partyData" / self.Struct_RF5_PARTYDATA,
            "itemData" / self.Struct_RF5_ITEMDATA,
            "farmSupportMonsterData" / self.Struct_RF5_FARMSUPPORTMONSTERDATA,
            "farmData" / self.Struct_RF5_FARMDATA,
            "statusData" / self.Struct_RF5_STATUSDATA,
            "orderData" / self.Struct_RF5_ORDERDATA,
            "makingData" / self.Struct_RF5_MAKINGDATA,
            "policeBatchData" / self.Struct_RF5_POLICEBATCHDATA,
            "itemFlagData" / self.Struct_RF5_ITEMFLAGDATA,
            "buildData" / self.Struct_RF5_BUILDDATA,
            "shippingData" / self.Struct_RF5_SHIPPINGDATA
        )



class Model_JP_1_0_7(Model_JP_1_0_6):
    def __init__(self):
        super().__init__()

        self.Struct_RF5_EVENTDATA: Struct = Struct(
            "eventSaveParameter" / self.Struct_EVENTSAVEPARAMETER,
            "saveFlag" / self.Struct_SAVEFLAG,
            "subEventSaveDatas" / self.Struct_SUBEVENTSAVEDATA,
            "mainScenarioStep" / Int32sl,
            "presentSendActorListCount" / Int32sl,
            "presentRecvActorListCount" / Int32sl,
            "isStartFishing" / Int8ul,
            "fesJoinActorListCount" / Int32sl,
            "fesVisitorActorListCount" / Int32sl,
            "fesNpcScoreListCount" / Int32sl,
            "isCalcFesId" / Int32sl,
            "victoryCandidateNpcId" / Int32sl, # Can be -1
            "judgeChildId" / Int32sl,
            "fishTypeListCount" / Int32sl,
            "unknown" / Int32sl[24] # Added
        )

        # Remember we need to update all the references to the new struct
        # definitions, otherwise they'll still be pointing to the old ones.
        self.Struct_RF5_DATA: Struct = Struct( # No change
            "slotNo" / Int32sl,
            "saveFlag" / self.Struct_SAVEFLAG,
            "worldData" / self.Struct_RF5_WORLDDATA,
            "playerData" / self.Struct_RF5_PLAYERDATA,
            "eventData" / self.Struct_RF5_EVENTDATA,
            "npcData" / self.Struct_RF5_NPCDATA,
            "fishingData" / self.Struct_RF5_FISHINGDATA,
            "stampData" / self.Struct_RF5_STAMPDATA,
            "furnitureData" / self.Struct_RF5_FURNITUREDATA,
            "partyData" / self.Struct_RF5_PARTYDATA,
            "itemData" / self.Struct_RF5_ITEMDATA,
            "farmSupportMonsterData" / self.Struct_RF5_FARMSUPPORTMONSTERDATA,
            "farmData" / self.Struct_RF5_FARMDATA,
            "statusData" / self.Struct_RF5_STATUSDATA,
            "orderData" / self.Struct_RF5_ORDERDATA,
            "makingData" / self.Struct_RF5_MAKINGDATA,
            "policeBatchData" / self.Struct_RF5_POLICEBATCHDATA,
            "itemFlagData" / self.Struct_RF5_ITEMFLAGDATA,
            "buildData" / self.Struct_RF5_BUILDDATA,
            "shippingData" / self.Struct_RF5_SHIPPINGDATA
        )


