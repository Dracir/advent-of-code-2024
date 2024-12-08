#[compute]
#version 450

// Invocations in the (x, y, z) dimension
layout(local_size_x = 1, local_size_y = 1, local_size_z = 1) in;

layout(set = 0, binding = 0, std430) restrict buffer LeftBuffer {
    float data[];
}
in_left_buffer;

layout(set = 0, binding = 1, std430) restrict buffer RightBuffer {
	float data[];
}
in_right_buffer;

layout(set = 0, binding = 2, std430) restrict buffer OutBuffer {
	float data[];
}
out_buffer;


void main() {
    // gl_GlobalInvocationID.x uniquely identifies this invocation across all work groups
	int same = 0;
	for(int i = 0; i < in_left_buffer.data.length(); i++) {
		if(in_left_buffer.data[gl_GlobalInvocationID.x] == in_right_buffer.data[i]) {
			same++;
		}
	}
	out_buffer.data[gl_GlobalInvocationID.x] = same * in_left_buffer.data[gl_GlobalInvocationID.x];
}