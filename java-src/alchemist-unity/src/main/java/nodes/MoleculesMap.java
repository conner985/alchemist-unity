package nodes;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/***
 * TODO.
 */
public class MoleculesMap {

    private final List<String> keys = new ArrayList<>();
    private final List<Object> values = new ArrayList<>();

    /***
     * @param mol TODO
     * @param conc TODO
     */
    public void addMolecule(final String mol, final Object conc) {
        if (keys.contains(mol)) {
            int index = keys.indexOf(mol);
            values.set(index, conc);
        } else {
            keys.add(mol);
            values.add(conc);
        }
    }

    /***
     * @param mol TODO
     * @return TODO
     */
    public Object getConcentration(final String mol) {
        if (keys.contains(mol)) {
            return values.get(keys.indexOf(mol));
        } else {
            return "";
        }
    }

    /***
     * 
     * @return TODO
     */
    public HashMap<String, Object> getMolecules() {
        if (keys == null || values == null) {
            return new HashMap<>();
        }

        HashMap<String, Object> moleculesMap = new HashMap<>();

        try {
            if (keys.size() != values.size()) {
                throw new Exception("inconsistent number of keys and values");
            }
            for (int i = 0; i < keys.size(); i++) {
                moleculesMap.put(keys.get(i), values.get(i));
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        return moleculesMap;
    }

}
