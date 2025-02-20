'use client';

import { useState, useEffect } from 'react';

import { clientErrors } from '@/constants/errors';
import { type Todo } from '@/types/todo';

const API_URL = '/api/todo';

export function useTodos(userId?: number, todoId?: number) {
  const [todoList, setTodoList] = useState<Todo[]>([]);
  const [todoItem, setTodoItem] = useState<Todo | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (todoId) {
      fetchTodoById(todoId);
    } else if (userId) {
      fetchTodosByUserId(userId);
    } else {
      fetchTodos();
    }
  }, [userId, todoId]);

  async function fetchTodos() {
    setLoading(true);
    setError(null);

    try {
      const response = await fetch(API_URL);

      if (!response.ok) throw new Error(clientErrors.TODO_GET_GENERIC);

      const data = await response.json();
      setTodoList(data);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  async function fetchTodoById(id: number) {
    setLoading(true);
    setError(null);

    try {
      const response = await fetch(`${API_URL}/${id}`);
      if (!response.ok) throw new Error(clientErrors.TODO_GET_NOT_FOUND);

      const data = await response.json();
      setTodoItem(data);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  async function fetchTodosByUserId(userId: number) {
    setLoading(true);
    setError(null);

    try {
      const response = await fetch(`${API_URL}?userId=${userId}`);
      if (!response.ok) throw new Error('No todos found for this user.');

      const data = await response.json();
      setTodoList(data);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  return { todoList, todoItem, loading, error, fetchTodos, fetchTodoById, fetchTodosByUserId };
}
