using rentasgt.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace rentasgt.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
