extends CharacterBody2D


const SPEED = 600.0
const ACCEL = 7
const NEST = Vector2(0, 0)
@onready
var nav: NavigationAgent2D = $NavigationAgent2D
var player
@onready
var map: NavigationRegion2D = $"../NavigationRegion2D"
var count = 10
var tick = 0
func _physics_process(delta: float) -> void:
	tick += delta
	if (tick <= count):
		return
	player=get_node("/root/main/bob").global_position
	var direction = Vector3()
	
	#var distance
	#distance=NavigationServer2D.map_get_closest_point(map.get_rid(), player).distance_to(player)
	nav.target_position = player
	
	direction = nav.get_next_path_position() - global_position  
	if direction.length()>50:#Hvis havfruen er langt nok unna målet (ikke spinning)
		look_at(nav.get_next_path_position())	

	direction = direction.normalized()
	
	velocity = velocity.lerp (direction * SPEED, ACCEL * delta)


	move_and_slide()
