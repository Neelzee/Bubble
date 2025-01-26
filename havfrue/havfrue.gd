extends CharacterBody2D


const SPEED = 600.0
const ACCEL = 7
const NEST = Vector2(0, 0)
@onready
var nav: NavigationAgent2D = $NavigationAgent2D
var player
@onready
var map: NavigationRegion2D = $"../NavigationRegion2D"
#func _ready() -> void:
#	player=get_node("../Bob").global_position
#print(player)
func _physics_process(delta: float) -> void:
	player=get_node("../Bob").global_position
	var direction = Vector3()
	
	#var distance
	#distance=NavigationServer2D.map_get_closest_point(map.get_rid(), player).distance_to(player)
	nav.target_position = player
	
	direction = nav.get_next_path_position() - global_position  
	direction = direction.normalized()
	velocity = velocity.lerp (direction * SPEED, ACCEL * delta)


	move_and_slide()
