
using UnityEngine;


public class MovingCloudController : MonoBehaviour
{

    [SerializeField] Transform[] positions;
    [SerializeField] float speed = 10f;

    private Transform _nextPosition;
    private int _positionIndex = 0;
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _facingLeft = true;


    // Start is called before the first frame update
    void Start()
    {
        _nextPosition = positions[_positionIndex];
        _rigidbody = GetComponent<Rigidbody2D>();
        CalculateDirection();
    }

    void Update()
    {
        if (Vector2.Distance(_nextPosition.position, transform.position) < 0.2f)
        {
            if (_positionIndex + 1 >= positions.Length)
            {
                _positionIndex = 0;
            }
            else
            {
                _positionIndex++;
            }
            _nextPosition = positions[_positionIndex];
            FlipIfNecessary();
        }
        CalculateDirection();

    }
    void FixedUpdate()
    {
        // starts moving towards the next position
        _rigidbody.velocity = _direction * speed;
    }

    private void CalculateDirection()
    {
        _direction = (_nextPosition.position - transform.position).normalized;
    }

    private void FlipIfNecessary()
    {
        CalculateDirection();
        if ((_direction.x > 0 && _facingLeft) || (_direction.x < 0 && !_facingLeft))
        {
            _facingLeft = !_facingLeft;
            var localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
