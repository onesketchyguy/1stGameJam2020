
namespace Missions {
    public class MissionDef
    {
        public  object GetTask() 
        {
            
            if (completed) return true;
            else return task;
        }

        public bool completed;
        public System.Action task;
    }
}