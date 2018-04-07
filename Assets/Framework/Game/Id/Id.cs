// Author:			Wolfgang Neumayer
// Creation Date:	05/04/2018

// Encapsulating task ID within a class allows me to change the type later on.
// For example, I can move from using long to string quite easily without having to rewrite a lot of code.

namespace framework.id
{
    [System.Serializable]
    public class Id
    {
        int id;

        public Id(int id)
        {
            this.id = id;
        }

        public bool IsValid()
        {
            return !this.Equals (INVALID);
        }

        public bool AssertIsValid()
        {
            bool condition = this.IsValid ();
            errorhandling.ErrorHandling.AssertIsTrue (condition, "Id is invalid!");

            return condition;
        }

        public override bool Equals(object obj)
        {
            Id other = obj as Id;
            if (other == null)
                return false;

            return this.id.Equals (other.id);
        }

        // Since we use the task Id as a dictionary key, the hash function for two "equal" task Ids must also be the same!
        public override int GetHashCode()
        {
            return this.id;
        }

        public static Id INVALID
        {
            get
            {
                return new Id (-1);
            }
        }
    }
}