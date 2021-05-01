#pragma once

class Color
{
public:
	Color ( short newR, short newG, short newB, short newA = 255 )
	{
		SetR ( newR );
		SetG ( newG );
		SetB ( newB );
		SetA ( newA );
	}
	virtual unsigned short GetA () { return A; }
	virtual unsigned short GetR () { return R; }
	virtual unsigned short GetG () { return G; }
	virtual unsigned short GetB () { return B; }
	virtual void SetA ( unsigned short newA )
	{
		if ( newA < 256 ) { A = newA; }
		else { A = 255; }
	}

	virtual void SetR (unsigned short newR)
	{
		if ( newR < 256 ) { R = newR; }
		else { R = 255; }
	}

	virtual void SetG (unsigned short newG)
	{
		if ( newG < 256 ) { G = newG; }
		else { G = 255; }
	}

	virtual void SetB (unsigned short newB)
	{
		if ( newB < 256 ) { B = newB; }
		else { B = 255; }
	}
protected:
	unsigned short A;
	unsigned short R;
	unsigned short G;
	unsigned short B;
};