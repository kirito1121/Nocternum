using UnityEngine; // Thư viện Unity cho các thành phần game

public class Fireball : MonoBehaviour // Định nghĩa class Fireball kế thừa MonoBehaviour
{
    [SerializeField] private float speed; // Tốc độ bay của fireball
    [SerializeField] private float delayBeforeAppear; // Thời gian delay trước khi xuất hiện

    private bool hit; // Đánh dấu fireball đã va chạm chưa
    private Animator animator; // Điều khiển animation
    private BoxCollider2D boxCollider; // Phát hiện va chạm
    private float direction; // Hướng bay của fireball (1: phải, -1: trái)
    private Rigidbody2D body; // Xử lý vật lý

    private bool isDelaying; // Đang trong trạng thái delay
    private float delayStartTime; // Thời điểm bắt đầu delay
    private SpriteRenderer spriteRenderer; // Quản lý hiển thị sprite

    private void Awake() // Hàm khởi tạo khi object được tạo
    {
        animator = GetComponent<Animator>(); // Lấy component Animator
        boxCollider = GetComponent<BoxCollider2D>(); // Lấy component BoxCollider2D
        body = GetComponent<Rigidbody2D>(); // Lấy component Rigidbody2D
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy component SpriteRenderer
    }

    private void OnEnable() // Hàm gọi khi object được kích hoạt
    {
        hit = false; // Reset trạng thái va chạm
        boxCollider.enabled = true; // Bật collider
        if (body != null) // Nếu có Rigidbody2D
        {
            body.linearVelocity = Vector2.zero; // Reset vận tốc vật lý
        }
        if (!isDelaying) // Nếu không delay
        {
            spriteRenderer.enabled = true; // Hiện sprite
        }
    }

    private void Update() // Hàm gọi mỗi frame
    {
        if (isDelaying) // Nếu đang delay xuất hiện
        {
            if (Time.time - delayStartTime >= delayBeforeAppear) // Đủ thời gian delay
            {
                spriteRenderer.enabled = true; // Hiện sprite
                boxCollider.enabled = true;    // Bật collider
                isDelaying = false; // Kết thúc trạng thái delay
            }
            return; // Không xử lý di chuyển khi đang delay
        }

        if (!hit) // Nếu chưa va chạm
        {
            float moveStep = speed * Time.deltaTime * direction; // Tính khoảng cách di chuyển mỗi frame
            transform.Translate(moveStep, 0, 0); // Di chuyển fireball
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Hàm xử lý va chạm
    {
        if (hit) return; // Nếu đã va chạm thì bỏ qua
        hit = true; // Đánh dấu đã va chạm
        boxCollider.enabled = false; // Tắt collider
        animator.SetTrigger("explosion"); // Kích hoạt animation va chạm
    }

    public void SetDirection(float _direction) // Hàm thiết lập hướng bay và delay xuất hiện
    {
        direction = _direction; // Lưu hướng bay
        hit = false; // Reset trạng thái va chạm
        boxCollider.enabled = false; // Tắt collider khi chưa xuất hiện

        float scaleX = Mathf.Abs(transform.localScale.x) * Mathf.Sign(_direction); // Tính scale theo hướng
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z); // Áp dụng scale

        delayStartTime = Time.time; // Lưu thời điểm bắt đầu delay
        isDelaying = true; // Đánh dấu đang delay
        spriteRenderer.enabled = false; // Ẩn sprite nhưng object vẫn active
    }

    private void Deactivate() // Hàm ẩn fireball
    {
        gameObject.SetActive(false); // Ẩn object fireball
    }
}