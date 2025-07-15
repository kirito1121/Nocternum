using UnityEngine; // Sử dụng các API của Unity

public class PlayerAttach : MonoBehaviour // Định nghĩa class PlayerAttach kế thừa MonoBehaviour để dùng trong Unity
{
    [SerializeField] private float attackCooldown; // Thời gian hồi chiêu giữa các lần tấn công
    [SerializeField] private Transform firepoint; // Vị trí bắn ra fireball
    [SerializeField] private GameObject[] fireballs; // Mảng chứa các fireball để tái sử dụng

    private Animator animator; // Đối tượng Animator để điều khiển animation tấn công
    private PlayerController playerController; // Đối tượng PlayerController để kiểm tra trạng thái tấn công
    private float cooldownTimer = Mathf.Infinity; // Biến theo dõi thời gian hồi chiêu, khởi tạo là vô cực để có thể tấn công ngay khi bắt đầu

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Lấy component Animator gắn trên player
        playerController = GetComponent<PlayerController>(); // Lấy component PlayerController gắn trên player
    }

    private void Update()
    {
        // Kiểm tra nếu nhấn chuột trái, hết thời gian hồi chiêu và có thể tấn công thì thực hiện tấn công
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerController.canAttack())
        {
            Attack(); // Gọi hàm tấn công
        }
        cooldownTimer += Time.deltaTime; // Tăng thời gian hồi chiêu theo thời gian thực
    }

    private void Attack()
    {
        animator.SetTrigger("attack"); // Kích hoạt animation tấn công
        cooldownTimer = 0; // Reset thời gian hồi chiêu về 0
        int fireballIndex = FindFireball();
        if (fireballIndex != -1)
        {
            fireballs[fireballIndex].SetActive(true);
            fireballs[fireballIndex].transform.position = firepoint.position;
            fireballs[fireballIndex].GetComponent<Fireball>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private int FindFireball()
    {
        // Tìm fireball chưa hoạt động để tái sử dụng
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i; // Trả về chỉ số fireball chưa hoạt động
            }
        }
        return -1; // Nếu không có fireball nào sẵn sàng, trả về -1
    }

}