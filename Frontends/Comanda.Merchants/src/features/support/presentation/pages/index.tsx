import { useEffect } from 'react';

export const SupportPage = () => {
  useEffect(() => {
    // Replace with your WhatsApp number
    const whatsappNumber = '5511999999999'; // Placeholder - replace with actual number
    const whatsappLink = `https://wa.me/${whatsappNumber}`;
    window.location.href = whatsappLink;
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold mb-4">Suporte</h1>
      <p className="text-muted-foreground">Redirecionando para WhatsApp...</p>
    </div>
  );
};
