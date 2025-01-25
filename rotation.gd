extends CharacterBody2D


var player
#func thingy(_delta: float) -> void:
#	_sprite.global_position = get_global_mouse_position()
func _ready() -> void:
	player=get_node("../Player").global_position
func _physics_process(_delta: float) -> void:
	position=get_global_mouse_position()
	var myVector=player-position
	rotation=myVector.angle()+PI/2
	move_and_slide()
