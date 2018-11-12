﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class TodoService : SubService
    {
        public TodoService(GraphService service) : base(service)
        {
        }

        internal override Task<T> MakeRequest<T>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.MakeRequest<T>("beta/" + url, m, parameters, header, content, statusCode);
        }

        internal override Task<IRestResponse> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.HandleRequest("beta/" + url, m, parameters, header, content, statusCode);
        }

        public async Task<List<TaskFolder>> GetFolders()
        {
            return (await MakeRequest<FolderResult>("me/outlook/taskFolders?$top=200")).Value;
        }

        public async Task<TaskFolder> GetFolder(string name)
        {
            return (await GetFolders()).First(i => i.Name == name);
        }

        public async Task<List<Todo>> GetTasks()
        {
            return (await MakeRequest<TaskResult>("me/outlook/tasks?$top=200")).Value;
        }

        public async Task<IEnumerable<Todo>> GetTasks(TaskFolder folder)
        {
            var tasks = await GetTasks();
            return tasks.Where(i => i.ParentFolderId == folder.Id);
        }

        public async Task<IEnumerable<Todo>> GetTasks(string folderName)
        {
            return await GetTasks(await GetFolder(folderName));
        }

        public async Task Delete(string id)
        {
            await HandleRequest($"me/outlook/tasks('{id}')", Call.DELETE, statusCode: HttpStatusCode.NoContent);
        }

        public Task Delete(Todo todo)
        {
            return Delete(todo.Id);
        }

        public async Task Complete(string id)
        {
            await HandleRequest($"me/outlook/tasks('{id}')/complete", Call.POST);
        }

        public Task Complete(Todo todo)
        {
            return Complete(todo.Id);
        }

        public async Task<Todo> Create(Todo todo)
        {
            return await MakeRequest<Todo>("me/outlook/tasks", Call.POST, content: todo);
        }

        public async Task Update(string id, Todo todo)
        {
            await HandleRequest($"me/outlook/tasks('{id}')", Call.PATCH, content: todo);
        }


        public async Task Update(Todo original, Todo newValues)
        {
            await Update(original.Id, newValues);
        }
    }
}
