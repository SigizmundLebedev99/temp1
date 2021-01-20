using MonoGame.Extended.Collections;

namespace temp1.Components
{
  class Effect
  {
    public int Expired;
    public string Name;
    public int MaxHP;
    public float Protection;

    public virtual Stats Apply(Stats stats){
      return new Stats(){
        MaxHp = stats.MaxHp + MaxHP,
        Protection = stats.Protection + Protection
      };
    }
  }

  class Effects
  {
    public Bag<Effect> List = new Bag<Effect>();
    public Stats Apply(Stats stats)
    {

      for (var i = 0; i < List.Count; i++)
      {
        stats = List[i].Apply(stats);
      }
      return stats;
    }
  }
}