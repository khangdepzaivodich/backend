using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace backend.Services.ChatService
{
    public class ChatMongoService
    {
        private readonly IMongoCollection<ChatMessage> _messagesCollection;

        public ChatMongoService(IConfiguration config)
        {
            // 1. Lấy chuỗi kết nối từ file appsettings.Development.json
            var connectionString = config.GetConnectionString("MongoDb");

            // 2. Tạo client kết nối lên MongoDB Atlas
            var mongoClient = new MongoClient(connectionString);

            // 3. Kết nối vào Database có tên "ChatStoreDB" (Nó sẽ tự tạo nếu chưa có)
            var mongoDatabase = mongoClient.GetDatabase("ChatStoreDB");

            // 4. Kết nối vào Collection (tương đương với Table) có tên "Messages"
            _messagesCollection = mongoDatabase.GetCollection<ChatMessage>("Messages");
        }

        // Lấy danh sách toàn bộ tin nhắn
        public async Task<List<ChatMessage>> GetAsync() =>
            await _messagesCollection.Find(_ => true).ToListAsync();

        // Thêm 1 tin nhắn mới
        public async Task CreateAsync(ChatMessage newMessage) =>
            await _messagesCollection.InsertOneAsync(newMessage);
    }
}