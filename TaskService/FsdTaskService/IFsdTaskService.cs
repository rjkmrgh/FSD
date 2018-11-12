using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FsdTaskService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IFsdTaskService
    {
        // TODO: Add your service operations here
        [OperationContract]
        void AddTask(TaskDetails task);
        [OperationContract]
        void UpdateTask(TaskDetails task);
        [OperationContract]
        List<TaskDetails> ViewTask();
        [OperationContract]
        void EndTask(int taskID);
        
    }


}
