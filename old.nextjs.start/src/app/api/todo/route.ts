import { NextResponse } from 'next/server';

import apiConstants from '@/constants/api';
import { serverErrors } from '@/constants/errors';

export async function GET(req: Request) {
  try {
    const { searchParams } = new URL(req.url);
    const userId = searchParams.get('userId');

    let url = `${apiConstants.SECURE_BASE_URL}/${apiConstants.TODO_V1}`;
    if (userId) {
      url += `?userId=${userId}`;
    }

    const response = await fetch(url, { cache: 'no-store' });

    if (!response.ok) {
      const errorMessage = await response.text();
      console.error('Error from External API:', errorMessage);

      return NextResponse.json({ error: serverErrors.TODO_GET_GENERIC }, { status: response.status });
    }

    const todos = await response.json();
    return NextResponse.json(todos);
  } catch (error) {
    return NextResponse.json({ error: `${serverErrors.INTERNAL_SERVER_ERROR}: ${error}` }, { status: 500 });
  }
}

export async function POST(req: Request) {
  try {
    const body = await req.json();
    const response = await fetch(`${apiConstants.SECURE_BASE_URL}/${apiConstants.TODO_V1}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body)
    });

    if (!response.ok) {
      return NextResponse.json({ error: serverErrors.TODO_POST }, { status: response.status });
    }
  } catch (error) {
    return NextResponse.json({ error: `${serverErrors.INTERNAL_SERVER_ERROR}: ${error}` }, { status: 500 });
  }
}
