package nodes;

import java.io.Serializable;

public class NodePosition2D implements Serializable {

	private float posx,posy;
	
	public float getPosx() {
		return posx;
	}

	public void setPosx(float posx) {
		this.posx = posx;
	}

	public float getPosy() {
		return posy;
	}

	public void setPosy(float posy) {
		this.posy = posy;
	}

	public NodePosition2D(float posx, float posy) {
		this.posx = posx;
		this.posy = posy;
	}
	
}
