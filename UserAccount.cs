namespace LiteDB
{
    public class UserAccount
    {
        public int Id {get; set;}
        public int AccNum {get; set;}
        public string AccName {get; set;}
        public int AccValue {get; set;}
        public bool IsActive {get; set;}
        public int MaxAccountNum {get; set;}

        public override string ToString()
        {
            return string.Format("Id={0}   accnum={1}   accname={2}   accvalue={3}   isactive={4}   maxaccountnum={5}", this.Id, this.AccNum, this.AccName, this.AccValue, this.IsActive, this.MaxAccountNum);
        }
    }
}