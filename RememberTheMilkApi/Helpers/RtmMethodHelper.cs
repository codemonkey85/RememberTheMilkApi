using RememberTheMilkApi.Objects;
using System.Collections.Generic;

namespace RememberTheMilkApi.Helpers
{
    public static class RtmMethodHelper
    {
        internal const string AddTaskMethod = @"rtm.tasks.add";
        internal const string UndoMethod = @"rtm.transactions.undo";
        internal const string GetListsListMethod = @"rtm.lists.getList";
        internal const string GetTasksListMethod = @"rtm.tasks.getList";

        public static RtmApiResponse GetListsList() => RtmConnectionHelper.SendRequest(GetListsListMethod, new Dictionary<string, string>());

        public static RtmApiResponse GetTasksList() => RtmConnectionHelper.SendRequest(GetTasksListMethod, new Dictionary<string, string>());

        public static RtmApiResponse AddTask(string timeline, string name, string listId = null, string parse = null, string parentTaskId = null)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"timeline", timeline},
                {"name", name},
            };

            if (listId != null)
            {
                parameters.Add("list_id", listId);
            }

            if (parse != null)
            {
                parameters.Add("parse", parse);
            }

            if (parentTaskId != null)
            {
                parameters.Add("parent_task_id", parentTaskId);
            }

            return RtmConnectionHelper.SendRequest(AddTaskMethod, parameters);
        }

        public static RtmApiResponse UndoTransaction(string timeline, string transactionId)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"timeline", timeline},
                {"transaction_id", transactionId},
            };

            return RtmConnectionHelper.SendRequest(UndoMethod, parameters);
        }
    }
}