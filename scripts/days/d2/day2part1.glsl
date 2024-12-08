#[compute]
#version 450

layout(local_size_x = 1, local_size_y = 1, local_size_z = 1) in;

layout(set = 0, binding = 0, std430) restrict buffer MyDataBuffer {
    float data[];
}
my_data_buffer;


void main() {
	uint y = gl_GlobalInvocationID.y;

	float answer =1;
	float direction = sign(my_data_buffer.data[y*1024+1]-my_data_buffer.data[y*1024]);

	if(direction == 0) {
		answer = 0;
	}

	for(int x = 0; x < 1023; x++) {
		float diff = my_data_buffer.data[y*1024+x+1]-my_data_buffer.data[y*1024+x];
		if(my_data_buffer.data[y*1024+x+1] == -1) 
			break;
		if(sign(diff) != direction) {
			answer=0;
		}
		else if( abs(diff) > 3) {
			answer=0;
		}else if( abs(diff) == 0) {
			answer=0;
		}
		my_data_buffer.data[y*1024+x] = answer;
	
	}

	my_data_buffer.data[y*1024] = answer;
}