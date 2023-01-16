<template>
  <v-container>
        <v-card-title>
          <span class="text-h5 ma-4"> {{ item.name }}</span>
          <v-spacer></v-spacer>
          <!--Menu from the top-right corner-->
          <v-menu bottom left>
            <template v-slot:activator="{ on, attrs }">
              <v-btn icon v-bind="attrs" v-on="on">
                <v-icon>mdi-dots-vertical</v-icon>
              </v-btn>
            </template>
            <v-list>
              <!--Edit button-->
              <v-row justify="center">
                <v-dialog v-model="dialog" width="40%">
                  <template v-slot:activator="{ on, attrs }">
                    <v-list-item v-bind="attrs" v-on="on" link>
                      <v-list-item-title
                        ><v-icon>mdi-pencil</v-icon>Edit</v-list-item-title
                      >
                    </v-list-item>
                  </template>
                  <ChangeDepartmentDialog
                    :departmentName="item.name"
                  />
                </v-dialog>
              </v-row>
              <!--Delete button-->
              <v-list-item
                @click="onDeleteDepartment(item.id)"
                link
              >
                <v-list-item-title
                  ><v-icon>mdi-delete</v-icon>Delete</v-list-item-title
                >
              </v-list-item>
            </v-list>
          </v-menu>
        </v-card-title>
        <v-divider></v-divider>
        <v-card-text>
          <v-row justify="center">
            <v-col>
              <h4>Specializations:</h4>
            </v-col>
            <v-col>
              <v-list-item-group
                v-if="specializations.length > 0"
              >
                <v-list-item
                  v-for="child in specializations"
                  :key="child.id"
                >
                  <v-list-item-content>
                    <v-list-item-title v-text="child.name"></v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
              </v-list-item-group>
              <p v-else>This department has no specializations defined!</p>
            </v-col>
          </v-row>
        </v-card-text>
  </v-container>
</template>
  
  <style lang="css" scoped>
.v-list-scroll {
  height: 200px;
  overflow-y: auto;
}
</style>
  
  <script lang="ts">
import Vue from "vue";
import StoreHelper from "@/store/store-helper";
import { EventBus } from "@/main";
import { EVENT_BUS_RELOAD_ORGANIZATIONS } from "@/shared/constants";
import ChangeDepartmentDialog from "./ChangeDepartmentDialog.vue";
import { DepartmentViewModel } from "@/modules/department";
import { SpecializationViewModule } from "@/modules/specialization";
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  components: {
    ChangeDepartmentDialog,
  },
  props:{
    // The current department
    item: Object as () => DepartmentViewModel
  },
  data() {
    return {
      // Flag for Change department dialog
      dialog: false,
      // Specializations list
      specializations: [] as SpecializationViewModule[]
    };
  },
  async created() {
    console.log(this.item)
    this.specializations = await storeHelper.specializationStore.loadSpecializationsByDepartmentId(this.item.id);
  },
  mounted: function () {
    /**
     * Emit an event using EventBus in order to update the treeview whenever the user add a new department or specialization
     */
    EventBus.$on(EVENT_BUS_RELOAD_ORGANIZATIONS, () => {

      EventBus.$off(EVENT_BUS_RELOAD_ORGANIZATIONS);
    });
  },
  methods: {
    /**
     * Delete department from db and store
     */
    async onDeleteDepartment(departmentId: string): Promise<void> {
      const response = await StoreHelper.departmentStore.removeDepartment(departmentId);

      if (response.isSuccess) {
        /**
         * @todo implement something
         */   
      } else {
        window.alert(response.error);
      }
    },
  },
});
</script>