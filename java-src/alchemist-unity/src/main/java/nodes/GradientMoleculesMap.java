package nodes;

/***
 * Class that contains all the molecules needed for a gradient simulation.
 * Molecule present:
 * enabled: bool
 * source: bool
 * data: double
 */
public class GradientMoleculesMap implements IMoleculesMap {

    private boolean enabled;
    private boolean source;
    private double data;

    @Override
    public Object getMoleculeConcentration(final String mol) {
        switch (mol) {
        case "enabled":
            return enabled;
        case "source":
            return source;
        case "data":
            return data;
        default:
            return null;
        }
    }

    @Override
    public void setMolecule(final String mol, final Object conc) {
        switch (mol) {
        case "enabled":
            if (conc.getClass().equals(Boolean.class)) {
                enabled = (boolean) conc;
            } else {
                System.out.println("concentration of enabled must be a bool type, instead is: " + conc.getClass());
            }
            break;
        case "source":
            if (conc.getClass().equals(Boolean.class)) {
                source = (boolean) conc;
            } else {
                System.out.println("concentration of source must be a bool type, instead is: " + conc.getClass());
            }
            break;
        case "data":
            if (conc.getClass().equals(Double.class)) {
                data = (double) conc;
            } else {
                System.out.println("concentration of data must be a double type, instead is: " + conc.getClass());
            }
            break;
        default:
            System.out.println("don't know that molecule! : " + mol);
            break;
        }
    }

    @Override
    public void setMolecules(final IMoleculesMap newMolecules) {
        enabled = (boolean) newMolecules.getMoleculeConcentration("enabled");
        source = (boolean) newMolecules.getMoleculeConcentration("source");
        data = (double) newMolecules.getMoleculeConcentration("data");
    }

}
