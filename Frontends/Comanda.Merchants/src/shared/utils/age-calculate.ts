export function ageCalculate(dataNascimento: string): string {
  let nascimento: Date;

  if (dataNascimento.includes('/') && !dataNascimento.includes('-')) {
    const [dia, mes, ano] = dataNascimento.split('/').map(Number);
    nascimento = new Date(ano, mes - 1, dia);
  } else {
    nascimento = new Date(dataNascimento);
  }

  if (isNaN(nascimento.getTime())) {
    return 'Data inválida';
  }

  const hoje = new Date();
  let idade = hoje.getFullYear() - nascimento.getFullYear();
  const m = hoje.getMonth() - nascimento.getMonth();

  if (m < 0 || (m === 0 && hoje.getDate() < nascimento.getDate())) {
    idade--;
  }

  return `${idade} anos`;
}
