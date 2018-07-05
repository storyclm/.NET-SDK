using Shared;
using SroryCLM.SDK.CLMAnalitycs;
using System;
using System.Linq;
using Xunit;

namespace StoryCLM.SDK.CLMAnalitycs.Test
{
    public class CLMAnalitycs
    {

        DateTime _start = new DateTime(2016, 1, 1, 0, 0, 0);
        DateTime _finish = new DateTime(2018, 7, 1, 0, 0, 0);
        string[] usersIds = { "31e806b7-56b2-4560-ad39-2fa1a382a9d2", "d0532be9-d6d8-4401-8155-01e309f87aa7", "b2b68ca3-4e7b-4e36-b29e-dbcc06585065" };
        string[] usersId = { "31e806b7-56b2-4560-ad39-2fa1a382a9d2" };
        int[] presIds = { 4991, 5358 };
        int[] presId = { 4991 };

        [Theory]
        [InlineData(0)]
        public async void GetSessions(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

            var sessions = await sclm.GetSessionsAsync();
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(_start, _finish, presIds);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(_start, usersIds: usersIds);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(finish: _finish, usersIds: usersIds);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(_start, _finish, presentationsIds: presIds);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(_start,  presentationsIds: presIds);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(finish: _finish, presentationsIds: presIds);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(_start, _finish, usersIds: usersIds);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(usersIds: usersIds);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(_start, _finish, presIds, usersIds, 0, 1000);
            Assert.True(sessions.Count() > 0);

            sessions = await sclm.GetSessionsAsync(presentationsIds: presIds);

            sessions = await sclm.GetSessionsAsync(_start, _finish, presId, usersId);
            Assert.True(sessions.Count() > 0);

        }

        [Theory]
        [InlineData(0)]
        public async void GetCustomEvents(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);

            var custonEvents = await sclm.GetCustomEventsAsync();
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(_start, _finish, presIds, usersIds, 0, 1000);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(_start, _finish, presIds);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(_start, _finish, usersIds: usersIds);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(usersIds: usersIds);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(_start, usersIds: usersIds);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(finish: _finish, usersIds: usersIds);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(_start, _finish, presentationsIds: presIds);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(_start, presentationsIds: presIds);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(finish: _finish, presentationsIds: presIds);
            Assert.True(custonEvents.Count() > 0);

            custonEvents = await sclm.GetCustomEventsAsync(presentationsIds: presIds);

            custonEvents = await sclm.GetCustomEventsAsync(_start, _finish, presId, usersId);
            Assert.True(custonEvents.Count() > 0);

        }


    }
}
