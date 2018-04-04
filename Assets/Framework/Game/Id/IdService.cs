// Author:			Wolfgang Neumayer
// Creation Date:	05/04/2018

namespace framework.id
{
    public interface IIdService
    {
        Id GenerateNewTaskId();
    }

    public class SimpleIdService : IIdService
    {
        int currentIdCounter;

        public SimpleIdService()
        {
            // TODO: Read current id counter from save data
        }

        public SimpleIdService(int startingValue)
        {
            this.currentIdCounter = startingValue;
        }

        public Id GenerateNewTaskId()
        {
            return new Id(this.currentIdCounter++);
        }
    }
}