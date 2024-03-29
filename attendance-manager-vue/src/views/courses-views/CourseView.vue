<template>
  <v-layout justify-center column>
    <!-- The tabs menu -->
    <v-tabs
      v-model="currentTab"
      background-color="transparent"
      color="black"
      centered
    >
      <v-tab> View courses</v-tab>
      <v-tab> Add new course</v-tab>
    </v-tabs>

    <v-tabs-items v-model="currentTab" class="pa-3 custom-remove-background">
      <v-tab-item>
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
        <div v-else-if="courses.length != 0">
          <v-btn class="dark_button white--text ma-6" @click="onReloadAsync">
            <v-icon>mdi-reload</v-icon> Reload courses
          </v-btn>
          <v-simple-table>
            <template v-slot:default>
              <thead>
                <tr>
                  <th class="text-left black--text text-h6">Course name</th>
                  <th class="text-left black--text text-h6">
                    Specialization name
                  </th>
                  <th class="text-left black--text text-h6">Reports linked</th>
                  <th class="text-left black--text text-h6">Last update</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in courses" :key="item.courseId">
                  <td>{{ item.name }}</td>
                  <td>{{ item.specializationName }}</td>
                  <td>{{ item.reportsLinked }}</td>
                  <td>{{ getRelativeTime(item.updatedOn) }}</td>
                  <td>
                    <v-menu bottom right offset-y>
                      <template v-slot:activator="{ on, attrs }">
                        <v-btn icon v-bind="attrs" v-on="on">
                          <v-icon>mdi-dots-vertical</v-icon>
                        </v-btn>
                      </template>
                      <v-list>
                        <v-dialog
                          v-model="updateDialog"
                          max-width="50%"
                          :fullscreen="isMobile"
                        >
                          <template v-slot:activator="{ on, attrs }">
                            <v-list-item v-bind="attrs" v-on="on">
                              <v-list-item-title>Edit</v-list-item-title>
                            </v-list-item>
                          </template>
                          <UpdateCourseDialog
                            class="pa-1"
                            :course="item"
                            @save="updateDialog = false"
                            @close="updateDialog = false"
                          />
                        </v-dialog>

                        <v-list-item @click="onDeleteCourseAsync(item)">
                          <v-list-item-title>Delete</v-list-item-title>
                        </v-list-item>
                      </v-list>
                    </v-menu>
                  </td>
                </tr>
              </tbody>
            </template>
          </v-simple-table>
        </div>
        <div v-else>
          <MessageComponent
            description="There is no course defined yet."
            fontSize="20px"
            fontWeight="bold"
            icon="mdi-information"
            :color="WARNING_AMBER_DARKEN_4"
          />
        </div>
      </v-tab-item>
      <v-tab-item>
        <CreateCourseComponent @save="currentTab = 0" />
      </v-tab-item>
    </v-tabs-items>
  </v-layout>
</template>

<style scoped>
.custom-remove-background {
  background-color: transparent;
}
</style>

<script lang="ts">
import Vue from "vue";
import storeHelper from "@/store/store-helper";
import { CourseViewModule } from "@/modules/view-modules";
import CreateCourseComponent from "@/components/course/CreateCourseComponent.vue";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import { Toastification } from "@/plugins/vue-toastification";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import UpdateCourseDialog from "@/components/course/UpdateCourseDialog.vue";
import moment from "moment";

export default Vue.extend({
  name: "CourseView",
  components: {
    CreateCourseComponent,
    MessageComponent,
    UpdateCourseDialog,
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      currentTab: 0,
      updateDialog: false,
      // Add a flag to track if data fetching is successful
      isFetchSuccessful: false,
      isLoading: true,
    };
  },
  computed: {
    courses: function (): CourseViewModule[] {
      return storeHelper.courseStore.courses;
    },
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
    },
  },
  created: async function () {
    // Fetch data with a timeout of 30 seconds
    const fetchDataWithTimeout = async () => {
      try {
        await storeHelper.courseStore.loadCoursesAsync(false);
        await storeHelper.userStore.loadCurrentUserInfoAsync();
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
  },
  methods: {
    onDeleteCourseAsync: async function (course: CourseViewModule): Promise<void> {
      if (
        confirm(
          "Are you sure that you want to delete this course? There must be some reports defined and all of those will also be deleted."
        )
      ) {
        const result = await storeHelper.courseStore.removeCourseAsync(
          course.courseId
        );

        if (result) {
          Toastification.success(`The course ${course.name} was deleted!`);
        }
      }
    },
    getRelativeTime(updateOn: string) {
      return moment(new Date(updateOn)).fromNow();
    },
    onReloadAsync: async function (): Promise<void> {
      await storeHelper.courseStore.loadCoursesAsync(true);
    },
  },
});
</script>