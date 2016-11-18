namespace BattleNetApi.Objects.WoW
{
    public class Talent
    {
        public TalentGridCoordinates TalentGridCoorindates { get; set; }

        public Spell Spell { get; set; }

        public Specialization SpecTalentBelongsTo { get; set; }
    }
}
