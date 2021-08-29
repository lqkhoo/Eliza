using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eliza.Model
{
    // Implementing this interface will allow a method
    // to iterate over the class' public properties
    // in a foreach loop.

    public interface IElizaEnumerableClass : IEnumerable
    {

        /*
        protected IEnumerable<object> GetFieldsOrdered()
        {
            Stack<Type> typeStack = new();
            Type objectType = this.GetType();
            do {
                typeStack.Push(objectType);
                objectType = objectType.BaseType;
            } while (objectType != null);

            while (typeStack.Count > 0) {
                objectType = typeStack.Pop();
                foreach (FieldInfo fieldInfo in objectType.GetFields()) {
                    if (!fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute))) {
                        yield return fieldInfo.GetValue(this);
                    }
                }
            }
        }
        */

        protected IEnumerable<Tuple<FieldInfo, object>> GetFieldsOrdered()
        {
            Stack<Type> typeStack = new();
            Type objectType = this.GetType();
            do {
                typeStack.Push(objectType);
                objectType = objectType.BaseType;
            } while (objectType != null);

            while (typeStack.Count > 0) {
                objectType = typeStack.Pop();
                foreach (FieldInfo fieldInfo in objectType.GetFields()) {
                    if (!fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute))) {
                        yield return new Tuple<FieldInfo, object>(fieldInfo, fieldInfo.GetValue(this));
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetFieldsOrdered().GetEnumerator();
        }
    }
}
