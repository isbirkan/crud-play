import type { Metadata } from 'next';

import './globals.css';

export const metadata: Metadata = {
  title: 'Awesome Todo App',
  description: 'This is just my awesome Todo App'
};

export default function RootLayout({
  children
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className="antialiased">{children}</body>
    </html>
  );
}
