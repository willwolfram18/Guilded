using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BattleNetApi.Objects.WoW
{
    public class GuildEmblem
    {
        #region Properties
        public int Icon { get; set; }

        public string IconColor { get; set; }

        public int IconColorId { get; set; }

        public int Border { get; set; }

        public string BorderColor { get; set; }

        public int BorderColorId { get; set; }

        public string BackgroundColor { get; set; }

        public int BackgroundColorId { get; set; }
        #endregion

        internal GuildEmblem(JObject guildEmblemJson)
        {
            AssignPrimitiveProperty(ge => ge.Icon, guildEmblemJson, "icon");
            AssignPrimitiveProperty(ge => ge.IconColorId, guildEmblemJson, "iconColorId");
            AssignPrimitiveProperty(ge => ge.IconColor, guildEmblemJson, "iconColor");
            AssignPrimitiveProperty(ge => ge.BackgroundColor, guildEmblemJson, "backgroundColor");
            AssignPrimitiveProperty(ge => ge.BackgroundColorId, guildEmblemJson, "backgroundColorId");
            AssignPrimitiveProperty(ge => ge.Border, guildEmblemJson, "border");
            AssignPrimitiveProperty(ge => ge.BorderColor, guildEmblemJson, "borderColor");
            AssignPrimitiveProperty(ge => ge.BorderColorId, guildEmblemJson, "borderColorId");
        }

        private void AssignPrimitiveProperty<TProperty>(Expression<Func<GuildEmblem, TProperty>> propertyExpression, JObject guildEmblemJson, string jsonKey)
        {
            var memberExpression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            propertyInfo.SetValue(this, guildEmblemJson[jsonKey].Value<TProperty>());
        }
    }
}
