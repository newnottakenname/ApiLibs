﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.MicrosoftGraph
{
    public partial class FolderResult : ValueResult<TaskFolder> { }

    public partial class TaskFolder
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("isOwner")]
        public bool IsOwner { get; set; }

        [JsonProperty("isShared")]
        public bool IsShared { get; set; }

        [JsonProperty("wellknownListName")]
        public string WellknownListName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class TaskResult : ValueResult<Todo> { }

    public partial class Todo
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("importance")]
        public string Importance { get; set; }

        [JsonProperty("isReminderOn")]
        public bool IsReminderOn { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("completedDateTime")]
        public DatetimeTimeZone CompletedDateTime { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("dueDateTime")]
        public DatetimeTimeZone DueDateTime { get; set; }

        [JsonProperty("reminderDateTime")]
        public DatetimeTimeZone ReminderDateTime { get; set; }

        [JsonProperty("linkedResources")]
        public List<LinkedResource> LinkedResources { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Todo todo &&
                   Id == todo.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public partial class LinkedResource
    {
        [JsonProperty("webUrl")]
        public Uri WebUrl { get; set; }

        [JsonProperty("applicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class TaskBody
    {
        public TaskBody(string body)
        {
            Content = body;
            ContentType = "html";
        }

        public TaskBody() { }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public partial class DatetimeTimeZone
    {
        public DatetimeTimeZone()
        {
            TimeZone = "UTC";
        }

        [JsonProperty("dateTime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }
    }
}
