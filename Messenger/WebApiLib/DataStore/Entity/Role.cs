namespace SEM4_Swagger.DataStore.Entity
{
    public partial  class Role
    {
        public UserRole RoleType { get; set; }
        public string Name { get; set; }
        public virtual List<UserEntity> Users { get; set;}
    }
}
