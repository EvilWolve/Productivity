// Author:			Wolfgang Neumayer
// Creation Date:	04/04/2018

namespace game.taskmanagement.data
{
    using framework.id;
    using game.taskmanagement.enumerations;

    [System.Serializable]
    public class TaskDefinition
    {
        public Id id = Id.INVALID;

        public string name;
        public string description;

        public TaskFrequency frequency;

        public TaskCompletionType completionType;
        public float completionAmount; // Completion amount for Checkbox is 1f, integers can still be floats but are treated differently

        public int completionLimit;

        public TaskReward[] rewards;

        public static TaskDefinition INVALID
        {
            get
            {
                return null;
            }
        }
    }
}