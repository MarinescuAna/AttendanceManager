<template>
  <v-layout column class="ma-3">
    <v-flex>
      <v-btn-toggle class="ma-2" rounded>
        <v-btn
          class="blue_grey_button"
          title="Save changes."
          :disabled="!saveChanges"
          @click="onSave"
          v-if="isTeacher"
        >
          <v-icon>mdi-floppy</v-icon>
        </v-btn>
        <v-btn class="blue_grey_button" @click="onLoadData">
          <v-icon>mdi-cached</v-icon>
        </v-btn>
      </v-btn-toggle>
    </v-flex>
    <v-layout v-if="involvements.length > 0" wrap>
      <v-simple-table class="ma-2 table-color">
        <template v-slot:default>
          <thead>
            <tr>
              <th class="text-left black--text text-md-h6">Collection</th>
              <th class="text-left black--text text-md-h6">Attendance</th>
              <th class="text-left black--text text-md-h6">Bonus Points</th>
              <th class="text-left black--text text-md-h6">Activity Type</th>
              <th class="text-left black--text text-md-h6">Updated on</th>
              <th class="text-left black--text text-md-h6">Updated by</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in involvements" :key="item.involvementId">
              <td>
                {{
                  item.title != null && item.title != ""
                    ? `${item.title}(${item.heldOn})`
                    : item.heldOn
                }}
              </td>
              <td>
                <v-simple-checkbox
                  v-model="item.isPresent"
                  :disabled="!isTeacher"
                  @click="onPresenceChanged(item)"
                ></v-simple-checkbox>
              </td>
              <td>
                <v-text-field
                  type="number"
                  v-model="item.bonusPoints"
                  :disabled="!isTeacher || !item.isPresent"
                  v-if="isTeacher"
                ></v-text-field>
                <span v-else>{{ item.bonusPoints }}</span>
              </td>
              <td>{{ getActivityTypeName(item.activityType) }}</td>
              <td :title="item.updateOn">
                {{ getRelativeTime(item.updateOn) }}
              </td>
              <td>{{ item.updateBy }}</td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>

      <v-simple-table class="ma-2 table-color">
        <template v-slot:default>
          <thead>
            <tr>
              <th class="text-left black--text text-md-h6">Activity Type</th>
              <th class="text-left black--text text-md-h6">
                Total attendances
              </th>
              <th class="text-left black--text text-md-h6">Total points</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(total, key) in resultsOverview" :key="key">
              <td class="text-left black--text font-weight-black">
                {{ getActivityTypeName(key) }}
              </td>
              <td>{{ total["attendances"] }}</td>
              <td>{{ total["bonusPoints"] }}</td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
    </v-layout>
    <v-layout v-else>
      <MessageComponent
        description="There is no activity registered to this report."
        icon="mdi-information"
        :color="WARNING_AMBER_DARKEN_4"
      />
    </v-layout>
  </v-layout>
</template>

<style scoped>
.table-color {
  background-color: transparent;
}
</style>

<script lang="ts">
import Vue from "vue";
import { CourseType, Role } from "@/shared/enums";
import AuthService from "@/services/auth.service";
import MessageComponent from "../shared-components/MessageComponent.vue";
import InvolvementService from "@/services/involvement.service";
import { Toastification } from "@/plugins/vue-toastification";
import moment from "moment";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import { InvolvementViewModule } from "@/modules/view-modules";

interface ResultsOverview {
  attendances: number;
  bonusPoints: number;
}

export default Vue.extend({
  name: "StudentAttendanceExpandedComponent",
  components: { MessageComponent },
  props: {
    userId: {
      type: String,
      required: true,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      resultsOverview: {},
      involvements: [] as InvolvementViewModule[],
      involvementsInit: [] as InvolvementViewModule[],
    };
  },
  computed: {
    /** Get the current user role */
    isTeacher: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[2];
    },
    saveChanges: function (): boolean {
      return InvolvementService.isChanged(
        this.involvementsInit,
        this.involvements
      );
    },
  },
  created: async function (): Promise<void> {
    await this.onLoadData();
  },
  methods: {
    getRelativeTime(updateOn: string) {
      return moment(new Date(updateOn)).fromNow();
    },
    /**If the student is not present, then the bonus points cannot be inserted */
    onPresenceChanged: function (item: InvolvementViewModule): void {
      if (!item.isPresent) {
        item.bonusPoints = 0;
      }
    },
    /**Get the data for the second table */
    getTotalInvolvements: function (): void {
      for (let i = 1; i <= 3; i++) {
        const filteredData = this.involvements.filter(
          (d) => d.activityType == i
        );
        this.resultsOverview[i] = {
          attendances: filteredData.filter((d) => d.isPresent).length,
          bonusPoints: filteredData.reduce(
            (acc, item) => acc + item.bonusPoints,
            0
          ),
        } as ResultsOverview;
      }
    },
    getActivityTypeName: function (type: number): string {
      return CourseType[type].toString();
    },
    onLoadData: async function (): Promise<void> {
      //get involvements
      this.involvements = await InvolvementService.getInvolvements(
        -1,
        this.isTeacher ? this.userId : "",
        false,
        !this.isTeacher,
        false
      );

      //copy all the involvements
      this.involvements.forEach((x) =>
        this.involvementsInit.push(Object.assign({}, x))
      );

      //compute the data for the second table
      this.getTotalInvolvements();
    },
    onSave: async function (): Promise<void> {
      //get all the involvements that was updated
      const studentsChanged = InvolvementService.getInvolvementChanges(
        this.involvementsInit,
        this.involvements
      );

      if (studentsChanged.length !== 0) {
        const response = await InvolvementService.addStudentsAttendances({
          involvements: studentsChanged,
        });

        if (!response) {
          Toastification.simpleError(
            "Something went wrong and not all the attendances was saved"
          );
        } else {
          this.involvementsInit = this.involvements;
          Toastification.success(
            "All the involvements were successfully saved and the rewards were added."
          );
        }
      }
    },
  },
});
</script>