using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float moveForce = 3f; // Speed of the player
    [SerializeField] private float jumpForce = 7f; // Jump force of the player
    private bool isGrounded; // Cần kiểm tra va chạm với mặt đất
    private Animator animator; // Biến để lưu Animator nếu cần sử dụng
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        body.gravityScale = 3f; // Đặt trọng lực cho Rigidbody2D
    }
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        // Chỉ thay đổi vận tốc X, giữ nguyên vận tốc Y
        body.linearVelocity = new Vector2(moveHorizontal * moveForce, body.linearVelocity.y);

        if (moveHorizontal > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveHorizontal < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        animator.SetBool("run", moveHorizontal != 0);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            animator.SetTrigger("jump"); // Kích hoạt trigger nhảy trong Animator
            isGrounded = false; // Đặt isGrounded thành false sau khi nhảy
        }
    }


    //Thêm hàm kiểm tra va chạm với mặt đất(ví dụ OnCollisionEnter2D)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player is grounded: " + isGrounded);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Player is grounded: " + isGrounded);
        }
    }
}
