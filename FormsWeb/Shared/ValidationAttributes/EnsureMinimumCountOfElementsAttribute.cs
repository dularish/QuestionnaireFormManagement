using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsWeb.Shared.ValidationAttributes
{
    public class EnsureMinimumCountOfElementsAttribute : ValidationAttribute
    {
        public int NumberOfElements { get; }

        public EnsureMinimumCountOfElementsAttribute(int numberOfElements)
        {
            NumberOfElements = numberOfElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;

            if(list != null)
            {
                return list.Count >= NumberOfElements;
            }

            return true;
        }
    }
}
