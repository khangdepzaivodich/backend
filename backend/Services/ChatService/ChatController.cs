using Microsoft.AspNetCore.Mvc;

namespace backend.Services.ChatService
{
    [Route("api/[controller]")]
    [ApiController]

    // Đây là 1 API Test nhanh gọn
    public class ChatController : ControllerBase
    {
        private readonly ChatMongoService _chatService;
        private readonly ChatRedisService _redisService;

        public ChatController(
            ChatMongoService chatService, 
            ChatRedisService redisService)
        {
            _chatService = chatService;
            _redisService = redisService;
        }

        // Lấy toàn bộ Log Chat (Test ở trình duyệt)
        [HttpGet]
        public async Task<ActionResult<List<ChatMessage>>> Get()
        {
            var messages = await _chatService.GetAsync();
            return Ok(messages);
        }

        // Tạo Test 1 tin nhắn
        [HttpPost("SendTestMessage")]
        public async Task<IActionResult> SendTestMessage()
        {
            var newMsg = new ChatMessage
            {
                SenderId = "C001",
                ReceiverId = "C002",
                Content = "Chào bạn, áo khoác bò mẫu mới còn hàng không?",
                Timestamp = DateTime.UtcNow
            };

            await _chatService.CreateAsync(newMsg);

            return Ok(new { Note = "Tin nhắn đã đẩy lên MongoDB Atlas thành công nhé!", newMsg });
        }

        // --- TEST REDIS ---

        // Test tạo trạng thái 1 User đang Online
        [HttpGet("SetOnline/{userId}")]
        public async Task<IActionResult> SetUserOnline(string userId)
        {
            await _redisService.SetUserOnlineAsync(userId);
            return Ok(new { 
                Note = "Thành công!", 
                Message = $"Đã đánh dấu user [{userId}] là đang Online. (Sẽ tự Offline sau 5 phút nếu không kích hoạt lại)." 
            });
        }

        // Test kiểm tra xem User đó có đang Online không? (để show chấm xanh/chấm xám trong Chat)
        [HttpGet("CheckOnline/{userId}")]
        public async Task<IActionResult> CheckUserOnline(string userId)
        {
            var isOnline = await _redisService.IsUserOnlineAsync(userId);
            return Ok(new { 
                UserId = userId, 
                TrangThai = isOnline ? "Đang Online (Chấm xanh) 🟢" : "Đang Offline (Chấm xám) ⚪" 
            });
        }
    }
}