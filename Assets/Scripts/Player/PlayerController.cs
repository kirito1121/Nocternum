using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private float moveForce = 3f; // Speed of the player
    private float jumpForce = 10f; // Jump force of the player
    private bool isGrounded; // Cần kiểm tra va chạm với mặt đất
    private Animator animator; // Biến để lưu Animator nếu cần sử dụng
    private float moveHorizontal; // Biến để lưu giá trị di chuyển ngang
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        body.gravityScale = 1f; ; // Đặt trọng lực cho Rigidbody2D
    }
    private void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        // Chỉ thay đổi vận tốc X, giữ nguyên vận tốc Y
        body.linearVelocity = new Vector2(moveHorizontal * moveForce, body.linearVelocity.y);

        if (moveHorizontal > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveHorizontal < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.gravityScale = 2.5f;
            Jump();
        }
            

        animator.SetBool("run", moveHorizontal != 0);
        animator.SetBool("isGrounded", isGrounded);
        //anim khi rơi
        animator.SetBool("isFalling", !isGrounded && body.linearVelocity.y < 0);

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

    public bool canAttack()
    {
        return isGrounded;
    }
}
