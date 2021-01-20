namespace temp1.Components
{
    class Attack
    {
        public int TargetEntityId;
        public int Damage;

        public Attack(int targetEntityId, int damage)
        {
            TargetEntityId = targetEntityId;
            Damage = damage;
        }
    }
}