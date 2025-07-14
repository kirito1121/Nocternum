using UnityEngine;

public class PlayerAttach : MonoBehaviour
{
    [SerializeField] private float attackCooldown; // Thời gian hồi chiêu
    private Animator animator; // Biến để lưu Animator nếu cần sử dụng
    private PlayerController playerController; // Biến để lưu PlayerController nếu cần sử dụng
    private float cooldownTimer = Mathf.Infinity; // Biến để theo dõi thời gian hồi chiêu

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerController.canAttack())
        {
            Attack();
        }
        cooldownTimer += Time.deltaTime; // Cập nhật thời gian hồi chiêu
    }

    private void Attack()
    {
        animator.SetTrigger("attack"); // Kích hoạt trigger tấn công trong Animator
        cooldownTimer = 0; // Đặt lại thời gian hồi chiêu
    }
}
