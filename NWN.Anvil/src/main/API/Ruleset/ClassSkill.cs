using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ClassSkill
  {
    private readonly CNWClass_Skill skill;

    public ClassSkill(CNWClass_Skill skill)
    {
      this.skill = skill;
    }

    public bool IsClassSkill => skill.bClassSkill.ToBool();

    public NwSkill Skill => NwSkill.FromSkillId(skill.nSkill)!;
  }
}
