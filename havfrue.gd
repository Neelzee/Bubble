extends CharacterBody2D


const SPEED = 600.0
const ACCEL = 7
const NEST = Vector2(0, 0)
@onready
var nav: NavigationAgent2D = $NavigationAgent2D
var player
@onready
var map: NavigationRegion2D = $"../NavigationRegion2D"
func _ready() -> void:
	print(position)
	player=get_node("../Bob").global_position
func _physics_process(delta: float) -> void:
	var direction = Vector3()
	

	if NavigationServer2D.map_get_closest_point(map.get_rid(), player).distance_to(player)>0.1:
		nav.target_position = player
	else:
		pass
	direction = nav.get_next_path_position() - global_position  
	direction = direction.normalized()
	velocity = velocity.lerp (direction * SPEED, ACCEL * delta)


	move_and_slide()
