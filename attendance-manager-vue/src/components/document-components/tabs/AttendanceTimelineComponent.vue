<template>
  <v-container>
    <v-layout justify-center class="pa-2">
      <v-flex md5 lg5 v-if="collections.length > 0">
        <div v-if="!isMobile">
          <v-timeline large>
            <v-timeline-item
              v-for="item in collections"
              :key="item.collectionId"
              :color="getColorByType(item.courseType)"
              small
            >
              <template v-slot:opposite>
                <span>{{ item.activityTime.replaceAll("/", ".") }}</span>
              </template>
              <v-card
                class="elevation-2"
                :color="getColorByType(item.courseType)"
                min-width="200px"
              >
                <v-card-title>
                  <span v-if="item.title != '' && item.title != null">{{
                    item.title
                  }}</span>
                  <span v-else>{{ item.courseType }}</span>
                </v-card-title>
                <v-card-subtitle v-if="item.title != '' && item.title != null">
                  {{ item.courseType }}
                </v-card-subtitle>
                <v-card-actions
                  ><v-btn
                    @click="onOpenAttendanceDialog(item)"
                    outlined
                    >Involvements
                  </v-btn></v-card-actions
                >
              </v-card>
            </v-timeline-item>
          </v-timeline>
        </div>
        <div v-else>
          <!-- diplay the bullets as a list when the user is on mobile-->
          <v-list color="transparent" dense>
            <v-list-item
              v-for="(item, index) in collections"
              :key="item.collectionId"
              @click="onOpenAttendanceDialog(item)"
            >
              <v-list-item-content>
                <v-divider v-if="index != 0" class="mb-2"></v-divider>
                <v-list-item-title
                  v-if="item.title != '' || item.title != null"
                  >{{ item.title }}</v-list-item-title
                >
                <v-list-item-title v-else
                  >{{ item.activityTime }} -
                  {{ item.courseType }}</v-list-item-title
                >
                <v-list-item-subtitle
                  v-if="item.title != '' || item.title != null"
                  >{{ item.activityTime }} -
                  {{ item.courseType }}</v-list-item-subtitle
                >
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </div>
      </v-flex>
      <v-flex v-else>
        <h3 class="pa-9">There is not collection available!</h3>
      </v-flex>
    </v-layout>
    <v-layout justify-center>
      <v-btn
        class="white--text"
        color="black"
        @click="addCollectionDialog = true"
        :disabled="noActivityAvailable"
        v-if="isTeacher"
      >
        Add collection
      </v-btn></v-layout
    >

    <!--Dialog for displaing the details about an attendance collection-->
    <v-dialog
      v-if="updateInvolvementsDialog"
      v-model="updateInvolvementsDialog"
      fullscreen
      hide-overlay
      scrollable
    >
      <UpdateInvolvementsDialog
        :collection="selectedCollection"
        @close-dialog="updateInvolvementsDialog = false"
      />
    </v-dialog>

    <!--Dialog for displaing the add new attendance form-->
    <v-dialog
      v-if="addCollectionDialog"
      v-model="addCollectionDialog"
      persistent
      max-width="50%"
      :fullscreen="isMobile"
    >
      <AddAttendanceDateDialog
        @close="oncloseAddCollectionDialog"
        @save="oncloseAddCollectionDialog"
      />
    </v-dialog>
  </v-container>
</template>

<script lang="ts">
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import AddAttendanceDateDialog from "@/components/document-components/dialogs/AddAttendanceDateDialog.vue";
import AuthService from "@/services/auth.service";
import { CourseType, Role } from "@/shared/enums";
import {
  AMBER_DARKEN_4,
  LABORATORY_COLOR,
  LECTURE_COLOR,
  NONE_COLOR,
  SEMINARY_COLOR,
} from "@/shared/constants";
import UpdateInvolvementsDialog from "@/components/document-components/dialogs/UpdateInvolvementsDialog.vue";
import { CollectionDto } from "@/modules/view-modules";

export default Vue.extend({
  name: "AttendanceTimelineComponent",
  components: {
    AddAttendanceDateDialog,
    UpdateInvolvementsDialog,
  },
  data: function () {
    return {
      AMBER_DARKEN_4,
      addCollectionDialog: false,
      updateInvolvementsDialog: false,
      selectedCollection: {},
    };
  },
  computed: {
    noActivityAvailable: function (): boolean {
      return (
        storeHelper.documentStore.report.noLaboratories ==
          storeHelper.documentStore.report.maxNoLaboratories &&
        storeHelper.documentStore.report.noLessons ==
          storeHelper.documentStore.report.maxNoLessons &&
        storeHelper.documentStore.report.noSeminaries ==
          storeHelper.documentStore.report.maxNoSeminaries
      );
    },
    collections: function (): CollectionDto[] {
      return storeHelper.documentStore.report.collections;
    },
    documentId: function (): number {
      return storeHelper.documentStore.report.reportId;
    },
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
    },
  },
  methods: {
    oncloseAddCollectionDialog: function (): void {
      this.addCollectionDialog = false;
    },
    onOpenAttendanceDialog: function (collection: CollectionDto): void {
      this.updateInvolvementsDialog = true;
      this.selectedCollection = collection;
    },
    getColorByType: function (activityType: string): string {
      if (activityType == CourseType[1]) {
        return LECTURE_COLOR;
      } else if (activityType == CourseType[2]) {
        return LABORATORY_COLOR;
      } else if (activityType == CourseType[3]) {
        return SEMINARY_COLOR;
      } else {
        return NONE_COLOR;
      }
    },
  },
});
</script>