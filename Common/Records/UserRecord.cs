/*
 * LambdaSharp (λ#)
 * Copyright (C) 2018-2020
 * lambdasharp.net
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;

namespace Demo.WebSocketsChat.Common.Records {

    public sealed class UserRecord : ARecord {

        //--- Class Methods ---
        public static Task<UserRecord> GetUserRecord(Table table, string id)
            => GetItemAsync<UserRecord>(table, USER_PREFIX + id, INFO);

        //--- Properties ---
        public override string PK => USER_PREFIX + UserId;
        public override string SK => INFO;
        public string UserId { get; set; }
        public string UserName { get; set; }

        //--- Methods ---
        public async Task<IEnumerable<ConnectionRecord>> GetConnectionsAsync(Table table) {
            var query = new QueryFilter("SK", QueryOperator.BeginsWith, CONNECTION_PREFIX);
            return await DoSearchAsync<ConnectionRecord>(table.Query(PK, query));
        }

        public async Task<IEnumerable<SubscriptionRecord>> GetSubscribedChannelsAsync(Table table) {

            // TODO: specify using the User-to-Channel (Subscription Record Index)
            var query = new QueryFilter("SK", QueryOperator.BeginsWith, CHANNEL_PREFIX);
            return await DoSearchAsync<SubscriptionRecord>(table.Query(PK, query));
        }
    }
}
