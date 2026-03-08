import { PageHeader } from '@/shared/components/global/page-header';
import { ChartAreaInteractive } from '../components/chart';
import { StatsCards } from '../components/stats';

export const DashboardPage = () => {
  return (
    <>
      <PageHeader title="Dashboard" />
      <div className="flex flex-col gap-4 py-4 md:gap-6 md:py-6">
        <StatsCards />
        <div className="px-4 lg:px-6">
          <ChartAreaInteractive />
        </div>
      </div>
    </>
  );
};
