<template>
  <v-layout wrap>
    <BadgeDashboardComponent
      v-for="badge in badges"
      :key="badge.id"
      :description="badge.description"
      :imagePath="badge.imagePath"
      :percentage="badge.percentage"
      :occurrences="badge.occurrences"
      :title="badge.title"
      :role="badge.role"
      :maxOccurrences="badge.maxAchievements"
    />
  </v-layout>
</template>

<script lang="ts">
import BadgeDashboardComponent from "@/components/shared-components/BadgeDashboardComponent.vue";
import { BadgeDashboardDto, ReportDashboardDto } from "@/modules/view-modules";
import { Role } from "@/shared/enums";
import Vue from "vue";

interface TempBadgeDashboardDto {
  id: number;
  badge: BadgeDashboardDto;
  percentage: number;
  occurrences: number;
  title: string;
  description: string;
  imagePath: string;
  role: string;
  maxAchievements: number;
}

export default Vue.extend({
  name: "BadgesDiagram",
  components: { BadgeDashboardComponent },
  props: {
    reports: {
      type: Array as () => ReportDashboardDto[],
    },
  },
  computed: {
    badges: function (): TempBadgeDashboardDto[] {
      return this.computeBadges();
    },
  },
  methods: {
    computeBadges: function (): TempBadgeDashboardDto[] {
      let result: TempBadgeDashboardDto[] = [];

      if (this.reports == undefined || this.reports == null || this.reports.length == 0) {
        return result;
      }

      const badges = this.reports.flatMap((r) => r.badges);

      badges.forEach((badge) => {
        const oldBadge = result.find((b) => b.id == badge.badgeType);

        if (oldBadge == undefined) {
          result.push({
            description: badge.description,
            id: badge.badgeType,
            imagePath: badge.imagePath,
            occurrences: badge.noAchievements,
            percentage: badge.percentageAchievements,
            title: badge.title,
            badge: badge,
            role: Role[badge.role],
            maxAchievements: badge.maxAchievements,
          });
        } else {
          oldBadge.occurrences += badge.noAchievements;
          oldBadge.percentage += badge.percentageAchievements;
          oldBadge.maxAchievements += badge.maxAchievements;
        }
      });

      result.forEach((r) => {
        r.percentage = Number.parseFloat(
          (r.percentage / this.reports.length).toFixed(2)
        );
      });

      result.sort((badge1, badge2) => {
        // Compare by role
        const nameComparison = badge1.role.localeCompare(badge2.role);
        if (nameComparison !== 0) {
          return nameComparison;
        }

        // If roles are equal, compare by occurrences
        return badge2.occurrences - badge1.occurrences;
      });

      return result;
    },
  },
});
</script>