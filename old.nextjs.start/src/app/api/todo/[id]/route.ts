import { NextResponse } from 'next/server';

import apiConstants from '@/constants/api';
import { serverErrors } from '@/constants/errors';

export async function GET(_req: Request, { params }: { params: { id: string } }) {
  try {
    const response = await fetch(`${apiConstants.BASE_URL}/${apiConstants.TODO_V1}/${params.id}`);

    if (!response.ok) {
      return NextResponse.json({ error: serverErrors.TODO_GET_GENERIC }, { status: response.status });
    }

    const todo = await response.json();
    return NextResponse.json(todo);
  } catch (error) {
    return NextResponse.json({ error: `${serverErrors.INTERNAL_SERVER_ERROR}: ${error}` }, { status: 500 });
  }
}
