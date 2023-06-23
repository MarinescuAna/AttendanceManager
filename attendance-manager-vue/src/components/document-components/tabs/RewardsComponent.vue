<template>
  <div v-if="isLoading">
    <v-layout justify-center>
      <v-progress-circular
        :size="100"
        :width="8"
        color="black"
        indeterminate
      ></v-progress-circular>
    </v-layout>
  </div>
  <v-layout class="ma-2" column v-else>
    <div v-if="isTeacher">
      <v-tabs
        v-model="currentTab"
        background-color="transparent"
        color="black"
        centered
        show-arrows
        align-with-title
      >
        <v-tab>Your rewards</v-tab>
        <v-tab>Student rewards</v-tab>
        <v-tab>Badges statistics</v-tab>
      </v-tabs>

      <v-tabs-items
        class="mt-3 custom-remove-background"
        v-model="currentTab"
        touchless
      >
        <v-tab-item
          ><CurrentUserRewardsComponent :rewards="userBadges"
        /></v-tab-item>
        <v-tab-item>
          <v-layout wrap>
            <v-flex lg8 md8 xs12>
              <div
                class="d-flex justify-center"
                v-if="customBadges.length != 0"
              >
                <BadgeComponent
                  v-for="badge in customBadges"
                  :key="badge.badgeId"
                  :badge="badge"
                  :displayAsCustomBadge="true"
                />
              </div>
              <MessageComponent
                description="You have no rewards defined by now!"
                :color="WARNING_AMBER_DARKEN_4"
                icon="mdi-information"
                fontSize="20px"
                fontWeight="bold"
                v-else
            /></v-flex>
            <v-flex lg4 md4 xs12>
              <v-card class="orange_background" :class="isMobile ? 'mt-3' : ''">
                <v-card-title>Create badge</v-card-title>
                <v-card-text>
                  <validation-observer v-slot="{ handleSubmit, invalid }">
                    <v-form @submit.prevent="handleSubmit(onCreateBadge)">
                      <v-layout column align-center>
                        <validation-provider
                          name="title"
                          v-slot="{ errors }"
                          :rules="rules.required"
                        >
                          <v-text-field
                            v-model="title"
                            type="text"
                            label="Badge title"
                            prepend-icon="mdi-format-title"
                            :error-messages="errors"
                            required
                            counter
                            color="black"
                            maxlength="128"
                            class="pa-3"
                            :class="
                              isMobile
                                ? 'custom-inputs-size-smaller-screen'
                                : 'custom-inputs-size-large-screen'
                            "
                          />
                        </validation-provider>
                        <v-select
                          v-model="selectedCourseType"
                          :items="courseType"
                          label="Activity type"
                          prepend-icon="mdi-school"
                          class="pa-3"
                          :class="
                            isMobile
                              ? 'custom-inputs-size-smaller-screen'
                              : 'custom-inputs-size-large-screen'
                          "
                          @change="onActivityChanged"
                          required
                        ></v-select>
                        <div
                          class="d-flex flex-column align-center"
                          v-if="selectedCourseType != ''"
                        >
                          <v-img
                            class="custom-image-display mb-2"
                            :src="
                              require(`@/assets/images/badges/${selectedBadge.image}`)
                            "
                          ></v-img>
                          <v-select
                            :items="badges"
                            label="Values to take into account"
                            item-text="name"
                            item-value="id"
                            prepend-icon="mdi-animation"
                            v-model="selectedBadge"
                            class="pa-3"
                            :class="
                              isMobile
                                ? 'custom-inputs-size-smaller-screen'
                                : 'custom-inputs-size-large-screen'
                            "
                            color="black"
                            return-object
                            required
                          ></v-select>

                          <label
                            class="black--text"
                            :class="
                              isMobile
                                ? 'custom-inputs-size-smaller-screen'
                                : 'custom-inputs-size-large-screen'
                            "
                            >Number of
                            {{ selectedBadge.name.toLowerCase() }} that students
                            need to achieve this badge</label
                          >
                          <v-slider
                            v-model="selectedNumber"
                            thumb-label="always"
                            class="mt-8"
                            :class="
                              isMobile
                                ? 'custom-inputs-size-smaller-screen'
                                : 'custom-inputs-size-large-screen'
                            "
                            :max="maxPossibleNumber"
                            color="black"
                            track-color="black"
                            min="1"
                            append-icon="mdi-plus"
                            prepend-icon="mdi-minus"
                          >
                          </v-slider>
                        </div>
                        <v-btn
                          width="50%"
                          @click="onCreateBadge"
                          :disabled="
                            invalid ||
                            selectedNumber < 1 ||
                            selectedCourseType == ''
                          "
                          large
                          class="dark_button white--text"
                          >Create</v-btn
                        >
                      </v-layout>
                    </v-form>
                  </validation-observer></v-card-text
                >
              </v-card>
            </v-flex>
          </v-layout>
        </v-tab-item>
        <v-tab-item>
          <BadgePercentageComponent />
        </v-tab-item>
      </v-tabs-items>
    </div>
    <CurrentUserRewardsComponent :rewards="rewards" v-else />
  </v-layout>
</template>
<style scoped>
.custom-inputs-size-large-screen {
  width: 500px;
}
.custom-inputs-size-smaller-screen {
  width: 300px;
}
.custom-remove-background {
  background-color: transparent;
}
.custom-image-display {
  max-width: 130px;
  max-height: 130px;
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.7);
}
</style>
<script lang="ts">
import Vue from "vue";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import CurrentUserRewardsComponent from "../CurrentUserRewardsComponent.vue";
import AuthService from "@/services/auth.service";
import { BadgeType, CourseType, Role } from "@/shared/enums";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import storeHelper from "@/store/store-helper";
import { BadgeViewModule } from "@/modules/view-modules";
import RewardService from "@/services/reward.service";
import { rules } from "@/plugins/vee-validate";
import BadgeComponent from "@/components/shared-components/BadgeComponent.vue";
import BadgeService from "@/services/badge.service";
import BadgePercentageComponent from "./BadgePercentageComponent.vue";
import { Toastification } from "@/plugins/vue-toastification";

interface BadgeDto {
  id: number;
  name: string;
  image: string;
}

export default Vue.extend({
  name: "RewardsComponent",
  data: function () {
    return {
      rules,
      WARNING_AMBER_DARKEN_4,
      currentTab: 0,
      title: "",
      selectedBadge: {} as BadgeDto,
      rewards: [] as BadgeViewModule[],
      badges: [] as BadgeDto[],
      courseType: [] as string[],
      selectedCourseType: "",
      selectedNumber: 0,
      maxPossibleNumber: 0,
      // Add a flag to track if data fetching is successful
      isFetchSuccessful: false,
      isLoading: true,
    };
  },
  components: {
    MessageComponent,
    CurrentUserRewardsComponent,
    BadgeComponent,
    BadgePercentageComponent,
  },
  computed: {
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    userBadges: function (): BadgeViewModule[] {
      return this.rewards.filter((d) => !d.isCustom);
    },
    customBadges: function (): BadgeViewModule[] {
      return this.rewards.filter((d) => d.isCustom);
    },
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.lgAndDown;
    },
  },
  created: async function (): Promise<void> {
    // Fetch data with a timeout of 30 seconds
    const fetchDataWithTimeout = async () => {
      try {
        this.rewards = await RewardService.getRewardsByReportIdAsync();
        this.isFetchSuccessful = true;
      } catch (error) {
        Toastification.simpleError("An error occurred during data fetching.");
      } finally {
        this.isLoading = false;
      }
    };

    // Start fetching data
    fetchDataWithTimeout();

    // Set a timeout to hide the loader if data is not fetched
    setTimeout(() => {
      if (!this.isFetchSuccessful) {
        this.isLoading = false;
        Toastification.simpleError("Data fetching timeout");
      }
    }, 30000);

    this.badges = [
      {
        id: BadgeType.CustomAttendanceAchieved,
        name: "Attendance",
        image: "custom_attendances.jpg",
      },
      {
        id: BadgeType.CustomBonusPointAchieved,
        name: "Bonus points",
        image: "custom_bonus_points.jpg",
      },
    ];
    this.selectedBadge = this.badges[0];

    Object.values(CourseType).forEach((value) => {
      if (Number.isInteger(value) && this.getMaxNumber(value) > 0) {
        this.courseType.push(CourseType[value]);
      }
    });
  },
  methods: {
    getMaxNumber: function (type): number {
      switch (type) {
        case CourseType.Laboratory:
          return storeHelper.documentStore.report.maxNoLaboratories;
        case CourseType.Lecture:
          return storeHelper.documentStore.report.maxNoLessons;
        case CourseType.Seminary:
          return storeHelper.documentStore.report.maxNoSeminaries;
        default:
          return 0;
      }
    },
    onCreateBadge: async function (): Promise<void> {
      const result = await BadgeService.createBadge({
        maxNumber: this.selectedNumber,
        badgeType: BadgeType[this.selectedBadge.id],
        title: this.title,
        type: this.selectedCourseType as unknown as CourseType,
      });

      if (result != null) {
        this.rewards.push(result);
      }
    },
    onActivityChanged: function (): void {
      this.maxPossibleNumber = this.getMaxNumber(
        CourseType[this.selectedCourseType as unknown as CourseType]
      );
    },
  },
});
</script>