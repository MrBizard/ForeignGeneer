using System;
using Godot;

public abstract class BaseCraft
{
    public Recipe recipe { get; private set; }
    protected Inventory _input;
    protected Inventory _output;
    public Timer craftTimer { get;  set; }
    public float craftProgress { get; protected set; } = 0;
    public bool isCrafting { get; protected set; } = false;
    protected Action _onCraftFinished;

    public BaseCraft(Recipe recipe, Inventory input, Inventory output = null)
    {
        this.recipe = recipe;
        _input = input;
        _output = output;

        // Initialisation du timer
        craftTimer = new Timer();
        craftTimer.OneShot = true;
        craftTimer.Autostart = false;
        craftTimer.Timeout += onCraftFinished;
    }

    /// <summary>
    /// Démarre le craft.
    /// </summary>
    public virtual bool startCraft(Action onCraftFinished)
    {
        if (!canContinue())
            return false;

        if (compareRecipe())
        {
            isCrafting = true;
            _onCraftFinished = onCraftFinished;

            craftTimer.WaitTime = recipe.duration;
            craftTimer.Start();
            resetCraftProgress();
            consumeResources();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Arrête le craft.
    /// </summary>
    public void stopCraft()
    {
        isCrafting = false;
        craftTimer.Stop();
        resetCraftProgress();
    }

    /// <summary>
    /// Met à jour la progression du craft.
    /// </summary>
    public void updateCraftProgress(double delta)
    {
        if (isCrafting)
        {
            craftProgress += (float)delta / recipe.duration;
            if (craftProgress >= 1.0f)
            {
                craftProgress = 1.0f;
            }
        }
    }

    /// <summary>
    /// Vérifie si le craft peut continuer.
    /// </summary>
    public virtual bool canContinue()
    {
        if (isCrafting)
            return false;

        if (_output == null)
            return true;

        var outputSlotItem = _output.getItem(0);
        return outputSlotItem == null ||
               outputSlotItem.getStack() + recipe.output.getStack() <= outputSlotItem.getResource().getMaxStack;
    }

    /// <summary>
    /// Réinitialise la progression du craft.
    /// </summary>
    protected void resetCraftProgress()
    {
        craftProgress = 0;
    }

    /// <summary>
    /// Appelé lorsque le timer de craft est terminé.
    /// </summary>
    protected void onCraftFinished()
    {
        if (addOutput())
        {
            _onCraftFinished?.Invoke();
        }
        isCrafting = false;
    }

    // Méthodes abstraites à implémenter par les classes dérivées
    protected abstract bool compareRecipe();
    protected abstract bool consumeResources();
    public abstract bool addOutput();

    public override string ToString()
    {
        return "\n recipe : " + recipe.ToString() + "\n _output: " + _output.ToString() + "\n _input: " + _input.ToString();
    }
}