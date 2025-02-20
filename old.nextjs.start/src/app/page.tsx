import { Metadata } from 'next';

import TodoList from './todoList';

export async function generateMetadata({
  searchParams
}: {
  searchParams: Record<string, string | string[]>;
}): Promise<Metadata> {
  const userId = typeof searchParams.userId === 'string' ? searchParams.userId : undefined;

  return {
    title: userId ? `Awesome Todo App - List for ${userId}` : 'Awesome Todo App - List',
    description: `View and manage tasks${userId ? ` for User ${userId}` : ''}`
  };
}

export default function TodoPage({ searchParams }: { searchParams: Record<string, string | string[]> }) {
  return <TodoList searchParams={searchParams} />;
}
