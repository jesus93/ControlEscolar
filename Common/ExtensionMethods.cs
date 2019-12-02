using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common
{
    public static class ExtensionMethods
    {
        public static string ValidateStringData(this string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new Exception($"El campo no puedo ser nulo o vacío");

            //Regular expresion to entire words
            var regExp = new Regex("/\b($word)\b/i");
            if (regExp.IsMatch(data))
                return data;
            else
                throw new Exception("No se cumplen las condiciones de datos");

        }

        public static MvcHtmlString DisplayColumnNameFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, IEnumerable<TClass> model, Expression<Func<TClass, TProperty>> expression)

        {

            var name = ExpressionHelper.GetExpressionText(expression);

            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(

                () => Activator.CreateInstance<TClass>(), typeof(TClass), name);



            var returnName = metadata.DisplayName;

            if (string.IsNullOrEmpty(returnName))

                returnName = metadata.PropertyName;



            return new MvcHtmlString(returnName);

        }
    }
}
